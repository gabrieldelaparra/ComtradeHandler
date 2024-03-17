using Microsoft.Extensions.DependencyInjection;

namespace ComtradeHandler.Wpf.App.Utils;

public static class FactoryExtensions
{
    public static void AddGenericFactory<T>(this IServiceCollection services)
        where T : class
    {
        services.AddTransient<T>();
        services.AddSingleton<Func<T>>(x => () => x.GetRequiredService<T>()!);
        services.AddSingleton<IFactory<T>, GenericFactory<T>>();
    }

    public static void AddGenericFactory<TInterface, TImplementation>(this IServiceCollection services)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        services.AddTransient<TInterface, TImplementation>();
        services.AddSingleton<Func<TInterface>>(x => () => x.GetRequiredService<TInterface>()!);
        services.AddSingleton<IFactory<TInterface>, GenericFactory<TInterface>>();
    }

    public static void AddScopedFactory<T>(this IServiceCollection services) where T : class
    {
        services.AddScoped<IScopeFactory<T>, ScopeFactory<T>>();
    }

    public static void AddScopedFactory<TInterface, TImplementation>(this IServiceCollection services)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        services.AddScoped<TInterface, TImplementation>();
        services.AddSingleton<IScopeFactory<TInterface>, ScopeFactory<TInterface>>();
    }
}
