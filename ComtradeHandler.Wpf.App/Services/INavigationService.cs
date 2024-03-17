using ComtradeHandler.Wpf.App.Core;

namespace ComtradeHandler.Wpf.App.Services;

public interface INavigationService
{
    event EventHandler<IViewModel>? NavigationChanged;
}
