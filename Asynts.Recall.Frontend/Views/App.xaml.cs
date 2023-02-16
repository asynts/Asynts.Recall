using System.Threading.Tasks;
using System;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;

using Asynts.Recall.Backend.Persistance;
using Asynts.Recall.Frontend.ViewModels;
using System.Windows.Threading;

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

            services.AddSingleton<IContentRepository>(services =>
            {
                var contentRepository = new MemoryContentRepository();
                contentRepository.LoadExampleData();
                return contentRepository;
            });

            services.AddSingleton<ISearchEngine, SearchEngine>();

            services.AddTransient<ContentListViewModel>();
            services.AddTransient<QueryBoxViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
