### Notes

-	If I start the application, the `PageSearch` view is empty.

-	I found that `SetSearchQuery` is called correctly and it produces a result with two items.

-	What is weird is that I can get it to work if I manually submit the result.

### Tasks

-	Add assertion to ensure that we are running in the UI synchronization context.

-	Go through the code step by step.

### Theories

-	I suspect, this is because I call `LoadPages` twice in that function.

-	I suspect, this is because the `LoadPages` runs on the wrong thread.

-	I suspect, this is becuase I call the `RelayCommand` manually, that might not run it in the correct
	synchronization context.
