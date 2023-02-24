### Notes

-	If I start the application, the `PageSearch` view is empty.

-	I found that `SetSearchQuery` is called correctly and it produces a result with two items.

-	What is weird is that I can get it to work if I manually submit the result.

-	I verified that the synchronous sections of `SetSearchQuery` are executed in the UI thread:

	```csharp
    _dispatcher.VerifyAccess();
	```

-	I tried commenting out this line to ensure that we call `LoadPages` only once:

	```csharp
	LoadPages(new List<PageData>());
	```

	It didn't make any difference to the behaviour.

-	I tried executing the command instead of calling the underlying function, this did not make any difference:

	```csharp
	pageSearchVM.SetSearchQueryCommand.Execute(pageSearchRoute);
    //pageSearchVM.SetSearchQuery(pageSearchRoute);
	```

-	It seems that we handle the `RouteChangedEvent` twice:

	```none
	[RoutingService.Navigate] location=PageSearchRouteData { RequiredTags = System.Collections.Generic.List`1[System.String], InterestingTerms = System.Collections.Generic.List`1[System.String], RawText =  }
	[PageSearchViewModel._routingService_RouteChangedEvent] route=PageSearchRouteData { RequiredTags = System.Collections.Generic.List`1[System.String], InterestingTerms = System.Collections.Generic.List`1[System.String], RawText =  }
	[MainWindowViewModel._routingService_RouteChangedEvent] route=PageSearchRouteData { RequiredTags = System.Collections.Generic.List`1[System.String], InterestingTerms = System.Collections.Generic.List`1[System.String], RawText =  }
	```

-	It seems that the cancellation doesn't work correctly:

	```none
	[RoutingService.Navigate] location=PageSearchRouteData { RequiredTags = System.Collections.Generic.List`1[System.String], InterestingTerms = System.Collections.Generic.List`1[System.String], RawText =  }
	[PageSearchViewModel._routingService_RouteChangedEvent] route=PageSearchRouteData { RequiredTags = System.Collections.Generic.List`1[System.String], InterestingTerms = System.Collections.Generic.List`1[System.String], RawText =  }
	[PageSearchViewModel.SetSearchQuery]: cancelling existing request
	[MainWindowViewModel._routingService_RouteChangedEvent] route=PageSearchRouteData { RequiredTags = System.Collections.Generic.List`1[System.String], InterestingTerms = System.Collections.Generic.List`1[System.String], RawText =  }
	[PageSearchViewModel.SetSearchQuery]: cancelling existing request
	[PageSearchViewModel.SetSearchQuery]: got result
	[PageSearchViewModel.SetSearchQuery]: got result
	```

	I would expect that one of them says `"aborted"` since the second should cancel the first request.

-	Somehow, this works:
	```csharp
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
        },  TaskScheduler.FromCurrentSynchronizationContext(), cancellationToken);
	```
	But this does not:
	```csharp
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
        }, scheduler: TaskScheduler.FromCurrentSynchronizationContext(), cancellationToken: cancellationToken);
	```

-	I realized that the code could be similified with `async` and `await`:

	```csharp
	async Task<IList<PageData>> SearchAsync(PageSearchRouteData searchQuery, CancellationToken cancellationToken = default)
    {
        var pages = await Task.Run(() =>
        {
            return Search(searchQuery, cancellationToken).ToList();
        }, cancellationToken: cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return pages;
    }
	```
	I verified that we are executing in the correct context:
	```csharp
	async Task<IList<PageData>> SearchAsync(PageSearchRouteData searchQuery, CancellationToken cancellationToken = default)
    {
        Debug.Assert(Dispatcher.FromThread(Thread.CurrentThread) != null);

        var pages = await Task.Run(() =>
        {
            Debug.Assert(Dispatcher.FromThread(Thread.CurrentThread) == null);
            return Search(searchQuery, cancellationToken).ToList();
        }, cancellationToken: cancellationToken);

        Debug.Assert(Dispatcher.FromThread(Thread.CurrentThread) != null);

        cancellationToken.ThrowIfCancellationRequested();

        return pages;
    }
    ```

### Tasks

-	Go through the code step by step.

### Theories

-	I suspect, that both the event handler in `MainWindowViewModel` and in `PageSearchViewModel` run.
	They cancel each other or clear each other.

-	I suspect, that we handle the event that updates the UI before the UI has attached it's own event handlers.
