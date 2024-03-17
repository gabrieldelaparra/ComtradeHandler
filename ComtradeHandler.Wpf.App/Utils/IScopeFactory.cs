using Microsoft.Extensions.DependencyInjection;

namespace ComtradeHandler.Wpf.App.Utils;

public interface IScopeFactory<T>
{
    (IServiceScope scope, T service) Create();
}
