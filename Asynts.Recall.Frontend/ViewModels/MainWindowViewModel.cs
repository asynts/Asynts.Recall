using CommunityToolkit.Mvvm.ComponentModel;

namespace Asynts.Recall.Frontend.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel()
    {
        contentListVM = new ContentListViewModel();
        queryBoxVM = new QueryBoxViewModel(contentListVM);
    }

    [ObservableProperty]
    private ContentListViewModel contentListVM;

    [ObservableProperty]
    private QueryBoxViewModel queryBoxVM;
}
