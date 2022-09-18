using MovingCompany.Models;

namespace MovingCompany.Modules.Interfaces;

public interface ICustomerService
{
    public Task<Customer?> GetCustomerByIDAsync(int customerID);
    public Task<IEnumerable<Customer>> GetCustomersAsync(params int[] IDs);
    public Task<Customer> CreateCustomerAsync(Customer customer);
    public Task UpdateCustomerAsync(int customerID, Customer customer);
    public Task DeleteCustomerAsync(int customerID);

}
