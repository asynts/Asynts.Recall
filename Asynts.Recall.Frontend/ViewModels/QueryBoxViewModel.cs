using System;
using System.Collections.Generic;
using System.Diagnostics;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Asynts.Recall.Backend.Persistance.Data;
using Asynts.Recall.Backend.Services;

namespace Asynts.Recall.Frontend.ViewModels;

public partial class QueryBoxViewModel : ObservableObject
{
    readonly ISearchService _searchService;
    public QueryBoxViewModel(ISearchService searchService)
    {
        _searchService = searchService;
    }

    [ObservableProperty]
    private string query = string.Empty;

    [ObservableProperty]
    private string rawQuery = string.Empty;

    [RelayCommand]
    public void SubmitQuery()
    {
        var searchQueryData = ParseQuery();
        _searchService.UpdateSearchQueryAsync(searchQueryData);
    }

    private SearchQueryData ParseQuery()
    {
        var interestingTerms = new List<string>();
        var requiredTags = new List<string>();

        var queryParts = Query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (var queryPart in queryParts)
        {
            if (queryPart.StartsWith('[') && queryPart.EndsWith(']')) {
                requiredTags.Add(queryPart.Substring(1, queryPart.Length - 2));
            } else
            {
                interestingTerms.Add(queryPart);
            }
        }

        return new SearchQueryData
        {
            InterestingTerms = interestingTerms,
            RequiredTags = requiredTags,
            RawTextQuery = RawQuery,
        };
    }
}
