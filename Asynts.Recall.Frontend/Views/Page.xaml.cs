using System.Windows.Controls;

using Microsoft.Extensions.DependencyInjection;

using Asynts.Recall.Frontend.ViewModels;
using System.Diagnostics;
using System.Windows.Navigation;

namespace Asynts.Recall.Frontend.Views
{
    /// <summary>
    /// Interaction logic for Page.xaml
    /// </summary>
    public partial class Page : UserControl
    {
        public PageViewModel ViewModel => (PageViewModel)DataContext;

        public Page()
        {
            InitializeComponent();
        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs eventArgs)
        {
            ViewModel.NavigateDetailsPage(NavigationService.GetNavigationService(this));
        }
    }
}
