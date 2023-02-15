using System;
using System.Text;
using System.Windows;
using Asynts.Recall.Backend.Persistance;
using Asynts.Recall.Frontend.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Asynts.Recall.Frontend
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IContentRepository>(services =>
            {
                var contentRepository = new MemoryContentRepository();
                contentRepository.LoadExampleData();
                return contentRepository;
            });


            services.AddSingleton<ISearchEngine, SearchEngine>();

            services.AddTransient<ContentListViewModel>();
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<QueryBoxViewModel>();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var mainWindowVM = serviceProvider.GetRequiredService<MainWindowViewModel>();

            var mainWindow = new MainWindow(mainWindowVM);
            mainWindow.Show();
        }
    }
}
