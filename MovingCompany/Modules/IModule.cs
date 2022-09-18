namespace Microsoft.AspNetCore.Builder;

public interface IModule
{
    int Version { get; }
    public Task<IServiceCollection> RegisterModuleAsync(IServiceCollection builder);
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
}
