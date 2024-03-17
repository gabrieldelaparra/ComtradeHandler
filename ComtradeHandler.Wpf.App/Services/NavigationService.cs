using ComtradeHandler.Wpf.App.Core;
using Microsoft.Extensions.Logging;

namespace ComtradeHandler.Wpf.App.Services;

public class NavigationService : INavigationService
{
    private readonly ILogger<INavigationService> _logger;

    public NavigationService(ILogger<INavigationService> logger)
    {
        _logger = logger;
    }

    public event EventHandler<IViewModel>? NavigationChanged;
}
