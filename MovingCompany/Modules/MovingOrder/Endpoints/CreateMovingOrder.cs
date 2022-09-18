using Microsoft.AspNetCore.Mvc;
using MovingCompany.Models;
using MovingCompany.Modules.Interfaces;

namespace MovingCompany.Modules.Endpoints;

public static class CreateMovingOrderEndpoint
{
    public static async Task<IResult> CreateMovingOrder([FromServices] IMovingOrderService orderService, [FromBody] MovingOrder order)
    {
        try
        {
            order = await orderService.CreateExpandedMovingOrderAsync(order);
            return Results.Created($"/orders/{order.ID}", order);

        } 
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
