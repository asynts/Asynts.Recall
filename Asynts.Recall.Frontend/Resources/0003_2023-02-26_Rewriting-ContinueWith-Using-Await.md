--- Metadata
{
	"id": "3c3d7dc5-664a-4076-b3e6-11e26ebbc253",
	"title": "",
	"tags: [ "type/debugging-protocol", "language/csharp/", "framework/wpf/", "project/recall/wpf/", "topic/asynchronous/" ]
}
--- Comment
This is an old debugging protocol.
--- Summary
We can use `async` and `await` to simplify code that uses `Task.ContinueWith`.
The trick here is, that the code in asynchronous functions runs in the calling synchronization context.
Thus we stay in the UI thread and can safely await tasks that are provided by the backend.
The backend can use `Task.Run` to create a task from a thread pool which will use a different synchronization context.
--- Details
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

-   I am trying to establish if `Dispatcher.BeginInvoke` will run it immediately if on the UI thread:

    -   The `Dispatcher.BeginInvoke` method will ultimately call `InvokeAsyncImpl` which calls `RequestProcessing`.
        https://github.com/dotnet/wpf/blob/abe481539b97838c607bbe7b32da59f03a0f4367/src/Microsoft.DotNet.Wpf/src/WindowsBase/System/Windows/Threading/Dispatcher.cs#L960

    -   This calls `CriticalRequestProcessing`.
        https://github.com/dotnet/wpf/blob/abe481539b97838c607bbe7b32da59f03a0f4367/src/Microsoft.DotNet.Wpf/src/WindowsBase/System/Windows/Threading/Dispatcher.cs#L2400

    -   This calls `UnsafeNativeMethods.TryPostMessage`.
        -   I suspect, that the dispatcher will run code immediately if it's on the UI thread instead of something like `setImmediate` in JavaScript.'

    -   According to the documentation this doesn't wait for the message to be processed.
        https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-postmessagew

    I conclude that it functions similar to `setImmediate` in JavaScript.

-   When I made that change with `await` I expected this to fix the issue with cancellation, but it doesn't seem like this worked:
    ```csharp
    [RoutingService.Navigate] location=PageSearchRouteData { RequiredTags = System.Collections.Generic.List`1[System.String], InterestingTerms = System.Collections.Generic.List`1[System.String], RawText =  }
    [PageSearchViewModel._routingService_RouteChangedEvent] route=PageSearchRouteData { RequiredTags = System.Collections.Generic.List`1[System.String], InterestingTerms = System.Collections.Generic.List`1[System.String], RawText =  }
    [PageSearchViewModel.SetSearchQuery]: cancelling existing request
    [MainWindowViewModel._routingService_RouteChangedEvent] route=PageSearchRouteData { RequiredTags = System.Collections.Generic.List`1[System.String], InterestingTerms = System.Collections.Generic.List`1[System.String], RawText =  }
    [PageSearchViewModel.SetSearchQuery]: cancelling existing request
    [PageSearchViewModel.SetSearchQuery]: got result
    [PageSearchViewModel.SetSearchQuery]: got result
    ```

-   I was able to resolve this issue with `Dispatcher.BeginInvoke`:
    ```csharp
    dispatcher.BeginInvoke(() =>
    {
        _routingService.Navigate(new PageSearchRouteData
        {
            RequiredTags = new List<string>(),
            InterestingTerms = new List<string>(),
            RawText = "",
        });
    });
    ```
    I don't understand, why this works.

-   This is what the new debug output looks like:
    ```none
    [RoutingService.Navigate] location=PageSearchRouteData { RequiredTags = System.Collections.Generic.List`1[System.String], InterestingTerms = System.Collections.Generic.List`1[System.String], RawText =  }
    [PageSearchViewModel._routingService_RouteChangedEvent] route=PageSearchRouteData { RequiredTags = System.Collections.Generic.List`1[System.String], InterestingTerms = System.Collections.Generic.List`1[System.String], RawText =  }
    [PageSearchViewModel.SetSearchQuery]: cancelling existing request
    [MainWindowViewModel._routingService_RouteChangedEvent] route=PageSearchRouteData { RequiredTags = System.Collections.Generic.List`1[System.String], InterestingTerms = System.Collections.Generic.List`1[System.String], RawText =  }
    [PageSearchViewModel.SetSearchQuery]: cancelling existing request
    [PageSearchViewModel._routingService_RouteChangedEvent] route=PageSearchRouteData { RequiredTags = System.Collections.Generic.List`1[System.String], InterestingTerms = System.Collections.Generic.List`1[System.String], RawText =  }
    [PageSearchViewModel.SetSearchQuery]: cancelling existing request
    [PageSearchViewModel.SetSearchQuery]: got result
    [PageSearchViewModel.SetSearchQuery]: got result
    [PageSearchViewModel.SetSearchQuery]: got result
    ```
    Now, it runs three times, which is very unexpected.

-   Another unrelated issue is that submitting an empty query after visiting details page doesn't work either.

-   In the constructor of `MainWindowViewModel` I assigned to `CurrentViewModel`, this is what created the other `PageSearchViewModel`.

    -   The new output afterwards:
        ```none
        [RoutingService.Navigate] location=PageSearchRouteData { RequiredTags = System.Collections.Generic.List`1[System.String], InterestingTerms = System.Collections.Generic.List`1[System.String], RawText =  }
        [MainWindowViewModel._routingService_RouteChangedEvent] route=PageSearchRouteData { RequiredTags = System.Collections.Generic.List`1[System.String], InterestingTerms = System.Collections.Generic.List`1[System.String], RawText =  }
        [PageSearchViewModel.SetSearchQuery]: cancelling existing request
        [PageSearchViewModel.SetSearchQuery]: got result
        [RoutingService.Navigate] location=PageSearchRouteData { RequiredTags = System.Collections.Generic.List`1[System.String], InterestingTerms = System.Collections.Generic.List`1[System.String], RawText =  }
        [MainWindowViewModel._routingService_RouteChangedEvent] route=PageSearchRouteData { RequiredTags = System.Collections.Generic.List`1[System.String], InterestingTerms = System.Collections.Generic.List`1[System.String], RawText =  }
        [PageSearchViewModel.SetSearchQuery]: cancelling existing request
        [PageSearchViewModel._routingService_RouteChangedEvent] route=PageSearchRouteData { RequiredTags = System.Collections.Generic.List`1[System.String], InterestingTerms = System.Collections.Generic.List`1[System.String], RawText =  }
        [PageSearchViewModel.SetSearchQuery]: cancelling existing request
        [PageSearchViewModel._routingService_RouteChangedEvent] route=PageSearchRouteData { RequiredTags = System.Collections.Generic.List`1[System.String], InterestingTerms = System.Collections.Generic.List`1[System.String], RawText =  }
        [PageSearchViewModel.SetSearchQuery]: cancelling existing request
        [PageSearchViewModel.SetSearchQuery]: got result
        [PageSearchViewModel.SetSearchQuery]: got result
        [PageSearchViewModel.SetSearchQuery]: got result
        ```

    -   It's again broken even with the dispatcher.

-   I found this line in `PageSearch.xaml.cs`:
    ```csharp
    DataContext = App.Current.Services.GetRequiredService<PageSearchViewModel>();
    ```

### Tasks

-   Create a minimal reproducible example.

-	Go through the code step by step.

-   Verify that the bindings are intact.

-   Look if there are multiple `PageSearchViewModel` objects.

-   Verify that the cancellation works as intended.

### Theories

-   I think, the key is to understand why the requests don't cancel each other.

-	I suspect, that both the event handler in `MainWindowViewModel` and in `PageSearchViewModel` run.
	They cancel each other or clear each other.

-	I suspect, that we handle the event that updates the UI before the UI has attached it's own event handlers.

-   I suspect, that there are multiple `PageSearchViewModel` objects.

-   I suspect, that I am using the wrong `CancellationToken`.

-   I suspect, that `RoutingService.Navigate` needs to run on the UI thread for some reason.

-   I suspect, that this got something to do with the `ContentControl`.

### Result

-   This was caused by the following line in `PageSearch.xaml.cs`:
    ```csharp
    DataContext = App.Current.Services.GetRequiredService<PageSearchViewModel>();
    ```
