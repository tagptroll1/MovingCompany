using Microsoft.AspNetCore.Mvc;
using MovingCompany.Models;
using MovingCompany.Modules.Interfaces;

namespace MovingCompany.Modules.Endpoints;

public sealed class CustomerCRUDEndpoints
{
    public static async Task<IResult> CRUDGetCustomerByID(
        [FromServices] ICustomerService customerService,
        [FromRoute] int customerID)
    {
        var result = await customerService.GetCustomerByIDAsync(customerID);

        if (result is null)
            return Results.NotFound();
        return Results.Ok(result);
    }  

    public static async Task<IResult> CRUDGetCustomers([FromServices] ICustomerService customerService)
    {
        return Results.Ok(await customerService.GetCustomersAsync());
    }

    public static async Task<IResult> CRUDCreateCustomer(
        [FromServices] ICustomerService customerService,
        [FromBody] Customer customer)
    {
        customer = await customerService.CreateCustomerAsync(customer);
        return Results.Ok(customer);
    }

    public static async Task<IResult> CRUDUpdateCustomer(
        [FromServices] ICustomerService customerService,
        [FromRoute] int customerID,
        [FromBody] Customer customer)
    {
        await customerService.UpdateCustomerAsync(customerID, customer);
        return Results.NoContent();
    }

    public static async Task<IResult> CRUDDeleteCustomer(
        [FromServices] ICustomerService customerService,
        [FromRoute] int customerID)
    {
        await customerService.DeleteCustomerAsync(customerID);
        return Results.NoContent();
    } 
}
