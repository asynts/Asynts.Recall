using System.Windows.Controls;

using Asynts.Recall.Frontend.ViewModels;

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
        }
    }
}
