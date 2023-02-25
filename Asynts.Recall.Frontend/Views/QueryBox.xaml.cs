using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

using Microsoft.Extensions.DependencyInjection;

using Asynts.Recall.Frontend.ViewModels;
using System.ComponentModel;

namespace Asynts.Recall.Frontend.Views
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

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = new QueryBoxViewModel();
            }
            else
            {
                DataContext = App.Current.Services.GetRequiredService<QueryBoxViewModel>();
            }
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
