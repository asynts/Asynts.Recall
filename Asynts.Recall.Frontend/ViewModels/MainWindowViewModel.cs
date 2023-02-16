using System;
using System.Diagnostics;

using Microsoft.Extensions.DependencyInjection;

using CommunityToolkit.Mvvm.ComponentModel;

using Asynts.Recall.Backend.Persistance;

namespace Asynts.Recall.Frontend.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel(IServiceProvider serviceProvider, IContentRepository contentRepository, ISearchEngine searchEngine)
    {
        this.contentListVM = ActivatorUtilities.CreateInstance<ContentListViewModel>(serviceProvider);
        this.queryBoxVM = ActivatorUtilities.CreateInstance<QueryBoxViewModel>(serviceProvider, contentListVM);
    }

    [ObservableProperty]
    private ContentListViewModel contentListVM;

    [ObservableProperty]
    private QueryBoxViewModel queryBoxVM;
}
