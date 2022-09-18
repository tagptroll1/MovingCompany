using Dapper;
using Microsoft.AspNetCore.Mvc;
using MovingCompany.Framework;
using MovingCompany.Models;
using MovingCompany.Modules.DataAccess;
using MovingCompany.Modules.Endpoints;
using MovingCompany.Modules.Interfaces;

namespace Microsoft.AspNetCore.Builder;

public sealed class CustomerModule : IModule
{
    public int Version => 1;

    public Task<IServiceCollection> RegisterModuleAsync(IServiceCollection builder)
    {
        builder.AddScoped<ICustomerService, CustomerService>();
        return Task.FromResult(builder);
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        #region CRUD
        endpoints.MapGet($"api/v{Version}/customers", CustomerCRUDEndpoints.CRUDGetCustomers);
        endpoints.MapPost($"api/v{Version}/customers", CustomerCRUDEndpoints.CRUDCreateCustomer);
        endpoints.MapGet($"api/v{Version}/customers/{{customerID:int}}", CustomerCRUDEndpoints.CRUDGetCustomerByID);
        endpoints.MapPut($"api/v{Version}/customers/{{customerID:int}}", CustomerCRUDEndpoints.CRUDUpdateCustomer);
        endpoints.MapDelete($"api/v{Version}/customers/{{customerID:int}}", CustomerCRUDEndpoints.CRUDDeleteCustomer);
        #endregion

        endpoints.MapGet($"api/v{Version}/customers/expanded", async ([FromServices] IConfiguration configuration) =>
        {
            using var connection = SqlConnectionFactory.GetConnection(configuration.GetConnectionString("Database"));
            return await connection.QueryAsync<Customer, Phone, Customer>(@"
                SELECT * FROM Customer c
                LEFT JOIN Phone p ON p.ID = c.PhoneID
                WHERE c.Deleted = false
            ", (customer, phone) =>
            {
                customer.Phone = phone;
                return customer;
            });
        });

        endpoints.MapPost(
            $"api/v{Version}/customers/expanded", async (
            [FromServices] IConfiguration configuration,
            [FromBody] Customer customer) =>
        {
            using var connection = SqlConnectionFactory.GetConnection(configuration.GetConnectionString("Database"));
            int? phoneID = customer.PhoneID;

            if (customer.Phone is not null && phoneID is null)
            {
                customer.PhoneID = await connection.InsertAsync(customer.Phone);
            }

            var identity = await connection.InsertAsync(customer);
            customer.ID = identity;
            return customer;
        });
        return endpoints;
    }
}
