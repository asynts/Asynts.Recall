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
        public QueryBox()
        {
            InitializeComponent();
        }

        public QueryBoxViewModel ViewModel => (QueryBoxViewModel)DataContext;

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ViewModel.SubmitQuery();
            }
        }
    }
}
