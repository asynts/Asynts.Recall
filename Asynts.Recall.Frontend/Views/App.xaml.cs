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

            // We keep critical sections in the backend synchronized with the UI.
            // To achieve this, expose the `TaskScheduler` used by WPF (which is non-concurrent.)
            services.AddSingleton<TaskScheduler>(services =>
            {
                // Unfortunately, there is no API that allows us to create a 'TaskScheduler' from a 'Dispatcher' directly.
                // Instead, we run 'FromCurrentSynchronizationContext' in the dispatcher.
                return Dispatcher.Invoke(() => TaskScheduler.FromCurrentSynchronizationContext());
            });

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
