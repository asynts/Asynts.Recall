### Notes

-	Somehow, the `Query` field in `QueryBoxViewModel` isn't updated.

-	The rest of the `SearchEngine` stuff is untested.

-	In order to resolve this issue, I am moving the current pending query into the `SearchEngine`.

	-	When the search has been completed, we emit an event.

	-	We cancel the previous request.

	-	In order to do that, the cancellation token needs to be checked on the correct thread.

-	For some reason there is a `Dispatcher` in WPF that is equivalent to `SynchronizationContext`.

-	My solution is to use the following snippet:

	```csharp
	// This will be used to ensure that critical sections do not run concurrently.
    services.AddSingleton<TaskScheduler>(TaskScheduler.FromCurrentSynchronizationContext());
	```

-	It is possible to create an service that isn't registered using:

	```csharp
	ActivatorUtilities.CreateInstance<UnregisteredClass>(serviceProvider);
	```

-	It seems that the community toolkit helps with dependency injection:
	https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/ioc

### Tasks

-	Read this article about how to access services from the viewmodel.
	https://docs.devexpress.com/WPF/17450/mvvm-framework/services/services-in-custom-viewmodels

-	Get rid of dependency injection by moving `SetSearchQuery` into the backend.

-	Research, how to get global information into view model.

-	Try using `Application.Current.ServiceProvider`.

### Theories

-	Maybe this is because we swap out the `DataContext` in the parent element?
