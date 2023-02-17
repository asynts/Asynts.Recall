using System.Threading.Tasks;
using System;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;

using Asynts.Recall.Backend.Persistance;
using Asynts.Recall.Frontend.ViewModels;
using System.Windows.Threading;
using System.Runtime.Serialization;
using Asynts.Recall.Backend.Services;

namespace Asynts.Recall.Frontend.Views
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;

        public IServiceProvider Services { get; private set; }

        public App()
        {
            Services = ConfigureServices();
        }

        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

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

            services.AddSingleton<ObjectIDGenerator>();

            services.AddTransient<PageSearchViewModel>();
            services.AddTransient<QueryBoxViewModel>();
            services.AddTransient<PageViewModel>();
            services.AddTransient<MainWindowViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
