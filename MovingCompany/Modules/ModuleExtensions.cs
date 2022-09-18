using System.Collections.Concurrent;

namespace Microsoft.AspNetCore.Builder;

public static class ModuleExtensions
{
    static readonly ConcurrentQueue<IModule> _registeredModules = new();

    /// <summary>
    /// Call RegisterModule on all modules inheriting the IModules interface
    /// Used for setting up the Module and DI
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static async Task<IServiceCollection> RegisterModules(this IServiceCollection services)
    {
        var modules = DiscoverModules();
        var tasks = new List<Task>();

        foreach (var module in modules)
        {
            tasks.Add(
                module.RegisterModuleAsync(services)
                    .ContinueWith(services =>
                    {
                        _registeredModules.Enqueue(module);
                        return services;
                    }));
        }

        await Task.WhenAll(tasks);
        return services;
    }

    /// <summary>
    /// Calls MapEndpoints on all modules registered.
    /// Registeres the modules endpoints to simple api
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        foreach (var module in _registeredModules)
        {
            module.MapEndpoints(app);
        }
        return app;
    }

    private static IEnumerable<IModule> DiscoverModules()
    {
        return typeof(IModule).Assembly
            .GetTypes()
            .Where(p => p.IsClass && p.IsAssignableTo(typeof(IModule)))
            .Select(Activator.CreateInstance)
            .Cast<IModule>();
    }
}
