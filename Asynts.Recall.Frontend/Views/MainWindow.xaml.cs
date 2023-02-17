using Asynts.Recall.Frontend.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Asynts.Recall.Frontend.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;

    public MainWindow()
    {
        InitializeComponent();

        DataContext = App.Current.Services.GetRequiredService<MainWindowViewModel>();
    }
}
