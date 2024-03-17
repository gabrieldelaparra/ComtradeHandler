using ComtradeHandler.Core.Models;
using Microsoft.Extensions.Logging;

namespace ComtradeHandler.Wpf.App.ViewModels;
public class ComtradeViewModel
{
    private readonly ILogger<ComtradeViewModel> _logger;

    public ComtradeViewModel(ILogger<ComtradeViewModel> logger)
    {
        _logger = logger;
    }

    private ComtradeConfiguration? ComtradeConfiguration { get; set; }
    private ComtradeData? ComtradeData { get; set; }

    
}
