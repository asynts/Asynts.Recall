using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Asynts.Recall.Backend.Persistance.Data;

namespace Asynts.Recall.Backend.Persistance;

public interface IPageRepository
{
    event EventHandler? LoadedEvent;

    IEnumerable<PageData> All();
    PageData GetByUuid(string uuid);

    // FIXME: This should not be here at all.
    Task LoadFromDiskAsync(string directoryPath, CancellationToken cancellationToken = default);
}
