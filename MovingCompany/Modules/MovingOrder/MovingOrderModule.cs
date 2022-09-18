
using MovingCompany.Modules.DataAccess;
using MovingCompany.Modules.Endpoints;
using MovingCompany.Modules.Interfaces;

namespace Microsoft.AspNetCore.Builder;

public sealed class MovingOrderModule : IModule
{
    public int Version { get => 1; }

    public Task<IServiceCollection> RegisterModuleAsync(IServiceCollection services)
    {
        services.AddScoped<IMovingOrderService, MovingOrdersService>();

        return Task.FromResult(services);
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        // CRUD
        endpoints.MapGet($"api/v{Version}/orders/{{orderID:int}}", MovingOrderCRUDEndpoints.CRUDGetMovingOrder)
            .WithName("Get MovingOrder by id");
        endpoints.MapPut($"api/v{Version}/orders/{{orderID:int}}", MovingOrderCRUDEndpoints.CRUDUpdateMovingOrder)
           .WithName("Update MovingOrder");
        endpoints.MapGet($"api/v{Version}/orders", MovingOrderCRUDEndpoints.CRUDGetMovingOrders)
           .WithName("Get all MovingOrder");
        endpoints.MapPost($"api/v{Version}/orders", MovingOrderCRUDEndpoints.CRUDCreateMovingOrder)
           .WithName("Create new Movingorder");
        endpoints.MapDelete($"api/v{Version}/orders/{{orderID:int}}", MovingOrderCRUDEndpoints.CRUDDeleteMovingOrder)
           .WithName("Delete Movingorder by id");
        // end

        endpoints.MapGet($"api/v{Version}/orders/expanded", GetMovingOrdersEndoint.GetMovingOrders)
            .WithName(nameof(GetMovingOrdersEndoint.GetMovingOrders));
        endpoints.MapGet($"api/v{Version}/orders/expanded/{{id:int}}", GetMovingOrdersEndoint.GetMovingOrderByID)
            .WithName(nameof(GetMovingOrdersEndoint.GetMovingOrderByID));
        endpoints.MapPost($"api/v{Version}/orders/expanded", CreateMovingOrderEndpoint.CreateMovingOrder)
            .WithName(nameof(CreateMovingOrderEndpoint.CreateMovingOrder));

        return endpoints;
    }
}
