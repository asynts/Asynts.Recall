using System.Windows.Controls;

using Microsoft.Extensions.DependencyInjection;

using Asynts.Recall.Frontend.ViewModels;
using System.Diagnostics;

namespace Asynts.Recall.Frontend
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

            DataContext = ActivatorUtilities.CreateInstance<ContentListViewModel>(App.Services);
            Debug.Assert(DataContext != null);
        }
    }
}
