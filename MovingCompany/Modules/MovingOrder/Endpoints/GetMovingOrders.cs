using Microsoft.AspNetCore.Mvc;
using MovingCompany.Models;
using MovingCompany.Modules.Interfaces;

namespace MovingCompany.Modules.Endpoints;

public sealed class GetMovingOrdersEndoint
{
    public static Task<IEnumerable<MovingOrder>> GetMovingOrders(
        [FromServices] IMovingOrderService orderService,
        [FromQuery] int customerID = 0)
    {
        return customerID == 0 
            ? orderService.GetMovingOrdersExpandedAsync()
            : orderService.GetMovingOrdersForCustomer(customerID);
    }

    public static async Task<IResult> GetMovingOrderByID([FromServices] IMovingOrderService orderService, [FromRoute] int id)
    {
        var order = await orderService.GetByIdAsync(id);

        if (order is null)
        {
            return Results.NotFound($"Could not find MovingOrder with ID {id}");
        }
        return Results.Ok(order);
    }
}
