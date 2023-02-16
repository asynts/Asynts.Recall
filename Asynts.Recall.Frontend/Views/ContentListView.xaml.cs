using System.Windows.Controls;

using Microsoft.Extensions.DependencyInjection;

using Asynts.Recall.Frontend.ViewModels;
using System.Diagnostics;

namespace Asynts.Recall.Frontend.Views
{
    /// <summary>
    /// Interaction logic for ContentListView.xaml
    /// </summary>
    public partial class ContentListView : UserControl
    {
        public ContentListViewModel ViewModel => (ContentListViewModel)DataContext;

        public ContentListView()
        {
            InitializeComponent();

            DataContext = App.Current.Services.GetRequiredService<ContentListViewModel>();
        }
    }
}
