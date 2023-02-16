using System.Threading.Tasks;
using System;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;

using Asynts.Recall.Backend.Persistance;

namespace Asynts.Recall.Frontend
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider? ServiceProvider { get; set; }

        public static IServiceProvider Services => ((App)Current).ServiceProvider!;

        private void ConfigureServices(IServiceCollection services)
        {
            // We keep critical sections in the backend synchronized with the UI.
            // To achieve this, expose the `TaskScheduler` used by WPF (which is non-concurrent.)
            services.AddSingleton<TaskScheduler>(TaskScheduler.FromCurrentSynchronizationContext());

            services.AddSingleton<IContentRepository>(services =>
            {
                var contentRepository = new MemoryContentRepository();
                contentRepository.LoadExampleData();
                return contentRepository;
            });

            services.AddSingleton<ISearchEngine, SearchEngine>();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
