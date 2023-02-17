using System.Windows.Controls;

using Microsoft.Extensions.DependencyInjection;

using Asynts.Recall.Frontend.ViewModels;
using System.Diagnostics;

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

            // FIXME: If I comment it out it works somehow.
            //
            //        I do not understand why this makes a difference because the debugger clearly shows that 'DataContext' is null
            //        when we assign here.
            //
            //        I suspect that it overrides it again and doesn't connect the event handlers properly.
            var vm = App.Current.Services.GetRequiredService<PageViewModel>();
            Debug.WriteLine($"[Page.Page] vm.id={vm.Id}");
            DataContext = vm;
        }
    }
}
