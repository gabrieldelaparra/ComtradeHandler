using ComtradeHandler.Wpf.App.Core;
using Microsoft.Extensions.Logging;

namespace ComtradeHandler.Wpf.App.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly ILogger<MainViewModel> _logger;

    public MainViewModel(ILogger<MainViewModel> logger)
    {
        _logger = logger;
    }
}
