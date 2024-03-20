using System.IO;
using System.Windows;
using ComtradeHandler.Wpf.App.Models;
using ComtradeHandler.Wpf.App.Services;
using ComtradeHandler.Wpf.App.Utils;
using ComtradeHandler.Wpf.App.ViewModels;
using ComtradeHandler.Wpf.App.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ComtradeHandler.Wpf.App;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        ShutdownMode = ShutdownMode.OnLastWindowClose;

        // Configuration
        Configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", false, true)
                        .Build();

        // ApplicationSettings
        ApplicationSettings = new ApplicationSettings();
        Configuration.GetSection(nameof(Models.ApplicationSettings)).Bind(ApplicationSettings);

        //Logging
        Log.Logger = new LoggerConfiguration()
                     .ReadFrom.Configuration(Configuration)
                     .CreateLogger();

        var hostBuilder = Host.CreateDefaultBuilder()
                              .ConfigureAppConfiguration((_, configurationBuilder) => configurationBuilder.AddConfiguration(Configuration))
                              .ConfigureServices((_, services) => ConfigureServices(services));

        hostBuilder.UseSerilog();
        AppHost = hostBuilder.Build();
    }

    private static IHost? AppHost { get; set; }
    private static IConfiguration? Configuration { get; set; }
    private static ApplicationSettings? ApplicationSettings { get; set; }

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging();
        services.Configure<ApplicationSettings>(Configuration!.GetSection(nameof(Models.ApplicationSettings)));

        //Main View
        services.AddSingleton<MainWindow>(provider => {
            using var scope = provider.CreateScope();

            var mainWindow = new MainWindow {
                DataContext = scope.ServiceProvider.GetRequiredService<MainViewModel>()
            };

            return mainWindow;
        });

        //ViewModels & Services Registration:
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<ComtradeViewModel>();
        //Navigation
        services.AddGenericFactory<INavigationService, NavigationService>();

        //Database
        //services.AddDbContext<MyDbContext>(MyDbContext.Configure);
        //services.AddScopedFactory<IDatabaseService, DatabaseService>();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        Log.Information($"Starting {ApplicationSettings?.ApplicationName}");

        await AppHost!.StartAsync();

        var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
        startupForm.Show();
        ConsoleHelper.HideConsole();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        Log.Information($"Closing {ApplicationSettings?.ApplicationName}");

        await AppHost!.StopAsync();
        await Log.CloseAndFlushAsync();

        base.OnExit(e);
    }
}
