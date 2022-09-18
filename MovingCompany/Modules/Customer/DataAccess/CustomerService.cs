using Dapper;
using Dapper.Contrib.Extensions;
using MovingCompany.Framework;
using MovingCompany.Models;
using MovingCompany.Modules.Interfaces;
using System.Text;

namespace MovingCompany.Modules.DataAccess;

public sealed class CustomerService : ICustomerService
{
    private readonly string _connectionString;

    public CustomerService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Database");
    }
    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        using var connection = SqlConnectionFactory.GetConnection(_connectionString);
        var identity = await connection.InsertAsync(customer);
        customer.ID = identity;

        return customer;
    }

    public async Task DeleteCustomerAsync(int customerID)
    {
        using var connection = SqlConnectionFactory.GetConnection(_connectionString);
        await connection.UpdateAsync(new Customer { ID = customerID, Deleted = true });
    }

    public Task<Customer?> GetCustomerByIDAsync(int customerID)
    {
        return GetCustomersAsync(customerID).ContinueWith(results => results.Result.FirstOrDefault());
    }

    public async Task<IEnumerable<Customer>> GetCustomersAsync(params int[] IDs)
    {
        var queryBuilder = new StringBuilder("SELECT * FROM Customer c WHERE c.Deleted = false");

        if (IDs is not null && IDs.Length > 0)
        {
            queryBuilder.Append(" AND c.ID = ANY(@IDs)");
        }

        using var connection = SqlConnectionFactory.GetConnection(_connectionString);
        return await connection.QueryAsync<Customer>(queryBuilder.ToString(), new { IDs });
    }

    public async Task UpdateCustomerAsync(int customerID, Customer customer)
    {
        customer.ID = customerID;

        using var connection = SqlConnectionFactory.GetConnection(_connectionString);
        var query = "UPDATE Customer SET Name = @Name, Email = @Email, PhoneID = @PhoneID WHERE ID = @ID";
        await connection.OpenAsync();
        await connection.ExecuteAsync(query, customer);
    }
}
