using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Services;

public interface ISearchService
{
    IEnumerable<PageData> Search(PageSearchRouteData searchQuery, CancellationToken cancellationToken = default);

    Task<IList<PageData>> SearchAsync(PageSearchRouteData searchQuery, CancellationToken cancellationToken = default)
    {
        return Task
            .Run<IList<PageData>>(() =>
            {
                return Search(searchQuery, cancellationToken).ToList();
            }, cancellationToken)
            // Ensure that we don't produce any result if the task has been cancelled.
            // This must happen in the calling synchronization context.
            .ContinueWith<IList<PageData>>((task, _) =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                return task.Result;
            }, TaskScheduler.FromCurrentSynchronizationContext(), cancellationToken);
    }
}
