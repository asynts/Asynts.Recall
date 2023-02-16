using Asynts.Recall.Backend.Persistance;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;

namespace Asynts.Recall.Frontend.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel(IContentRepository contentRepository, ISearchEngine searchEngine)
    {
        this.contentListVM = new ContentListViewModel(contentRepository, searchEngine);
        this.queryBoxVM = new QueryBoxViewModel(contentListVM);

        Debug.WriteLine($"[MainWindowViewModel.MainWindowViewModel], contentListVM={this.contentListVM}, queryBoxVM={queryBoxVM}");
    }

    [ObservableProperty]
    private ContentListViewModel contentListVM;

    [ObservableProperty]
    private QueryBoxViewModel queryBoxVM;
}
