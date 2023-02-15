using System.Windows.Controls;

using Asynts.Recall.Frontend.Models;

namespace Asynts.Recall.Frontend
{
    /// <summary>
    /// Interaction logic for ContentListView.xaml
    /// </summary>
    public partial class ContentListView : Page
    {
        public ContentListView()
        {
            InitializeComponent();

            DataContext = new ContentListModel();
        }
    }
}
