using System.Windows.Controls;

using Microsoft.Extensions.DependencyInjection;

using Asynts.Recall.Frontend.ViewModels;
using System.Diagnostics;
using System.Windows.Navigation;
using System.Windows;
using CommunityToolkit.Mvvm.Input;

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

        public bool ShowDetails
        {
            get => (bool)GetValue(ShowDetailsProperty);
            set => SetValue(ShowDetailsProperty, value);
        }

        public static readonly DependencyProperty ShowDetailsProperty = DependencyProperty.Register(
            name: nameof(ShowDetails),
            propertyType: typeof(bool),
            ownerType: typeof(Page)
        );

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs eventArgs)
        {
            ViewModel.ShowDetailsPage();
        }
    }
}
