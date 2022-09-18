
namespace Microsoft.AspNetCore.Builder;

public sealed class AddressModule : IModule
{
    public int Version { get => 1; }

    public Task<IServiceCollection> RegisterModuleAsync(IServiceCollection services)
    { 
        //services.AddSingleton(new AddressService()); 

        return Task.FromResult(services);
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        //endpoints.MapGet($"api/v{Version}/address", () =>
        //{

        //}).WithName("GetAddresses");

        return endpoints;
    }
}
