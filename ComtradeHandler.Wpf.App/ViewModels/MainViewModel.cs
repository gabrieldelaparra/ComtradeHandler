using ComtradeHandler.Wpf.App.Core;
using Microsoft.Extensions.Logging;

namespace ComtradeHandler.Wpf.App.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly ILogger<MainViewModel> _logger;
    private readonly ComtradeViewModel _comtradeViewModel;

    public MainViewModel(ILogger<MainViewModel> logger, ComtradeViewModel comtradeViewModel)
    {
        _logger = logger;
        _comtradeViewModel = comtradeViewModel;
        CurrentViewModel = _comtradeViewModel;
    }
    public IViewModel? CurrentViewModel { get; set; }
}
