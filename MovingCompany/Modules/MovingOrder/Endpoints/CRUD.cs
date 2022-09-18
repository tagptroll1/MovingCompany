using Microsoft.AspNetCore.Mvc;
using MovingCompany.Models;
using MovingCompany.Modules.Interfaces;

namespace MovingCompany.Modules.Endpoints;

public sealed class MovingOrderCRUDEndpoints
{
    public static async Task<IResult> CRUDCreateMovingOrder([FromServices] IMovingOrderService orderService, [FromBody] MovingOrder order)
    {
        try
        {
            order = await orderService.CreateMovingOrderAsync(order);
            return Results.Created($"/orders/{order.ID}", order);

        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> CRUDUpdateMovingOrder(
        [FromServices] IMovingOrderService orderService,
        [FromRoute] int orderID,
        [FromBody] MovingOrder order)
    {
        try
        {
            await orderService.UpdateMovingOrder(orderID, order);
            return Results.NoContent();

        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> CRUDGetMovingOrder(
    [FromServices] IMovingOrderService orderService,
    [FromRoute] int orderID)
    {
        try
        {
            var order = await orderService.GetByIdAsync(orderID);
            return Results.Ok(order);

        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> CRUDGetMovingOrders(
        [FromServices] IMovingOrderService orderService)
    {
        try
        {
            var orders = await orderService.GetMovingOrdersAsync();
            return Results.Ok(orders);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> CRUDDeleteMovingOrder(
    [FromServices] IMovingOrderService orderService,
    [FromRoute] int orderID)
    {
        try
        {
            await orderService.DeleteMovingOrderAsync(orderID);
            return Results.NoContent();

        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
