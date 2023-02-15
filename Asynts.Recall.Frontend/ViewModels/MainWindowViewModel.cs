using Asynts.Recall.Backend.Persistance;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Asynts.Recall.Frontend.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel(ContentListViewModel contentListVM, QueryBoxViewModel queryBoxVM)
    {
        this.queryBoxVM = queryBoxVM;
        this.contentListVM = contentListVM;
    }

    [ObservableProperty]
    private ContentListViewModel contentListVM;

    [ObservableProperty]
    private QueryBoxViewModel queryBoxVM;
}
