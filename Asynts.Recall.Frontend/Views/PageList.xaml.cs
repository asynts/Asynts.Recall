using System.Windows.Controls;

using Microsoft.Extensions.DependencyInjection;

using Asynts.Recall.Frontend.ViewModels;
using System.Diagnostics;

namespace Asynts.Recall.Frontend.Views
{
    /// <summary>
    /// Interaction logic for ContentListView.xaml
    /// </summary>
    public partial class PageList : UserControl
    {
        public PageListViewModel ViewModel => (PageListViewModel)DataContext;

        public PageList()
        {
            InitializeComponent();

            DataContext = App.Current.Services.GetRequiredService<PageListViewModel>();
        }
    }
}
