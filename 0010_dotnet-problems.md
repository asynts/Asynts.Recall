### Notes

-	I am trying to catalog the major issues I ran into with dotnet.

### WPF

-	It's very unclear how the bindings are resolved and the order of evaluation seems to matter a lot more than it should.

	-	The constructor seems to run before the dependency properties are processed, however, setting values in the constructor can break binding sometimes.

	-	Binding to properties of sibling nodes seems to be unreliable as well.

-	Dependency properties are extremely error prone because it looks at the type and silently discards it if it doesn't match.

### .NET

-	The `CancellationToken` abstraction does not have any sort of synchronization mechanism.

	-	I should research how this is done in C++.
		If I remember correctly, `std::condition_variable` and `std::mutex` can achieve this together.
