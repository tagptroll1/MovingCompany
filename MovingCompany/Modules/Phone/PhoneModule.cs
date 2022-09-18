
namespace Microsoft.AspNetCore.Builder;

public sealed class PhoneModule : IModule
{
    public int Version => 1;
    public Task<IServiceCollection> RegisterModuleAsync(IServiceCollection services)
    { 
        return Task.FromResult(services);
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        return endpoints;
    }
}
