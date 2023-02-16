using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Asynts.Recall.Frontend.ViewModels;

namespace Asynts.Recall.Frontend
{
    /// <summary>
    /// Interaction logic for QueryBox.xaml
    /// </summary>
    public partial class QueryBox : UserControl
    {
        public QueryBoxViewModel ViewModel => (QueryBoxViewModel)DataContext;

        public QueryBox()
        {
            InitializeComponent();

            // FIXME: Need access to dependency injection here.
            DataContext = new QueryBoxViewModel();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs eventArgs)
        {
            if (eventArgs.Key == Key.Enter)
            {
                ViewModel.SubmitQuery();
            }
        }
    }
}
