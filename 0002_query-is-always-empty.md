### Notes

-	Somehow, the `Query` field in `QueryBoxViewModel` isn't updated.

-	The rest of the `SearchEngine` stuff is untested.

### Tasks

-	Get rid of dependency injection by moving `SetSearchQuery` into the backend.

### Theories

-	Maybe this is because we swap out the `DataContext` in the parent element?
