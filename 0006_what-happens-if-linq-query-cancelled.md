### Notes

-	I got this query:

	```csharp
	// We allow the search to be cancelled in the middle.
    return Search(searchQuery)
        .AsParallel()
        .WithCancellation(cancellationToken)
        .ToList();
	```

    What happens when the `cancellationToken` is used?

-   It seems to throw an `OperationCancelledException` which should be caught.
