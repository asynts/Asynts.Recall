### Notes

-	I want something similar to stack overflow where the navigation is based on the search bar.

-	I could add a navigation service that looks like this:

	```csharp
	void NavigateToSearch(SearchQueryData searchQueryData);
	void NavigateToPageDetails(long pageId);
	void NavigateToTagTree(IList<string> requiredTags);
	void NavigateBack();
	```

	However, I feel that this API would be very inconsistent.

-	We could handle this in the view model, but I can't get access to the `NavigationService` from the `Frame` in any reliable way.
	
	-	I tried using a `DependencyProperty`, but the constructor will run before that will be set to anything.

-	Essentially, I need a state machine that can be in three states: `Search{ SearchQueryData }`, `Page{ PageId }` or `Tags{ RequiredTags }`.
	One method would be to add more stuff to `SearchQueryData` or to add a wrapper type that can contain any of them.
