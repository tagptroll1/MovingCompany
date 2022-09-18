
using Dapper;
using Npgsql;

var allowFrontendPolicyName = "_myAllowFrontendPolicy";

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCors(options =>
{
    
    options.AddPolicy(name: allowFrontendPolicyName,
                      policy =>
                      {
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                          policy.AllowAnyOrigin();
                      });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Module services
var shouldCreateDatabase = args.FirstOrDefault(arg => arg.StartsWith("--createdatabase")) != null;
await SetUpDatabase(builder.Configuration, shouldCreateDatabase);
await builder.Services.RegisterModules();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(allowFrontendPolicyName);
//app.UseAuthorization();

// Domain based modules
app.MapEndpoints();

app.Run();



async Task SetUpDatabase(IConfiguration configuration, bool createDatabase = false)
{
    var sqlPath = "initTables";
    var connectionString = configuration.GetConnectionString("Database");

    var sqlBuilder = new NpgsqlConnectionStringBuilder(connectionString);

    if (createDatabase)
    {
        var configDatabaseName = sqlBuilder.Database;
        sqlBuilder.Database = "postgres";

        using (var createDatabaseConnection = new NpgsqlConnection(sqlBuilder.ConnectionString))
        {
            // I wont say this is a good idea.
            await createDatabaseConnection.ExecuteAsync($"CREATE DATABASE {configDatabaseName}");
        }

        sqlBuilder.Database = configDatabaseName;
    }

    using var connection = new NpgsqlConnection(sqlBuilder.ConnectionString);
    await connection.OpenAsync();
    var transaction = await connection.BeginTransactionAsync();

    // Enumerate in order by index value in filename `1_name` 
    var files = Directory.EnumerateFiles(sqlPath)
        .Select(f => new 
            { 
                fullname = f, 
                // fetches last name in the path, splits the name by _ to get the leading digit
                index = f.Split(@"\").Last().Split('_').First()
            });

    foreach (var file in files.OrderBy(name => Convert.ToInt32(name.index)))
    {
        //var text = await File.ReadAllTextAsync(file.fullname);
        //await connection.ExecuteAsync(text, transaction: transaction);
        var executeTask = await File.ReadAllTextAsync(file.fullname);
        await connection.ExecuteAsync(executeTask, transaction: transaction);
    }

    await transaction.CommitAsync();
}

