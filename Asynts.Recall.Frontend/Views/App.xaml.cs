using System.Threading.Tasks;
using System;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;

using Asynts.Recall.Backend.Persistance;
using Asynts.Recall.Frontend.ViewModels;
using System.Windows.Threading;
using System.Runtime.Serialization;
using Asynts.Recall.Backend.Services;
using Microsoft.Extensions.Logging;

namespace Asynts.Recall.Frontend.Views
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ILogger _logger;

        public new static App Current => (App)Application.Current;

        public IServiceProvider Services { get; private set; }

        public App()
        {
            Services = ConfigureServices();
            _logger = Services.GetRequiredService<ILogger<App>>();

            _logger.LogInformation("STARTUP");
        }

        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddDebug();
                builder.SetMinimumLevel(LogLevel.Debug);
            });

            // Allow the backend to dispatch to the event loop.
            services.AddSingleton<Dispatcher>(Dispatcher);

            services.AddSingleton<IPageRepository>(services =>
            {
                var contentRepository = new MemoryPageRepository();
                contentRepository.LoadExampleData();
                return contentRepository;
            });

            services.AddSingleton<ISearchService, SearchService>();
            services.AddSingleton<IRoutingService, RoutingService>();
            services.AddTransient<IPageParserService, PageParserService>();

            services.AddSingleton<ObjectIDGenerator>();

            services.AddTransient<PageViewModelFactory>();
            services.AddTransient<PageSearchViewModel>();
            services.AddTransient<QueryBoxViewModel>();
            services.AddTransient<MainWindowViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
