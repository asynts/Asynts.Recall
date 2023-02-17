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

            DataContext = App.Current.Services.GetRequiredService<PageSearchViewModel>();
        }

        public RelayCommand<long>? ShowPageDetailsCommand { get; set; }

        public static readonly DependencyProperty ShowPageDetailsCommandProperty = DependencyProperty.Register(
            name: nameof(ShowPageDetailsCommand),
            propertyType: typeof(RelayCommand),
            ownerType: typeof(PageSearch)
        );
    }
}
