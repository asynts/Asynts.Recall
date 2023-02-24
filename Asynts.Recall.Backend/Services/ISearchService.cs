using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Diagnostics;
using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Services;

public interface ISearchService
{
    IEnumerable<PageData> Search(PageSearchRouteData searchQuery, CancellationToken cancellationToken = default);

    async Task<IList<PageData>> SearchAsync(PageSearchRouteData searchQuery, CancellationToken cancellationToken = default)
    {
        var pages = await Task.Run(() =>
        {
            return Search(searchQuery, cancellationToken).ToList();
        }, cancellationToken: cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return pages;
    }
}
