using System;
using System.Collections.Generic;
using System.Diagnostics;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Frontend.Models;

internal partial class QueryBoxModel : ObservableObject
{
    [ObservableProperty]
    private string query = string.Empty;

    [ObservableProperty]
    private string rawQuery = string.Empty;

    [RelayCommand]
    private void SubmitQuery()
    {
        var searchQueryData = ParseQuery();

        // FIXME: Do something.
        Debug.WriteLine("Updating query!");
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
