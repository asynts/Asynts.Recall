using System.Windows.Controls;

using Microsoft.Extensions.DependencyInjection;

using Asynts.Recall.Frontend.ViewModels;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace Asynts.Recall.Frontend.Views
{
    /// <summary>
    /// Interaction logic for ContentListView.xaml
    /// </summary>
    public partial class PageSearch : UserControl
    {
        public PageSearchViewModel ViewModel => (PageSearchViewModel)DataContext;

        public PageSearch()
        {
            InitializeComponent();
        }
    }
}
