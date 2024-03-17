using Microsoft.Extensions.DependencyInjection;

namespace ComtradeHandler.Wpf.App.Utils;

public class ScopeFactory<T> : IScopeFactory<T> where T : class
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ScopeFactory(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public (IServiceScope scope, T service) Create()
    {
        var scope = _serviceScopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<T>();
        return (scope, service);
    }
}
