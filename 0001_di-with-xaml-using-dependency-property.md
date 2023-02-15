### Notes

-	I want to do dependency injection where the `MainWindowViewModel` provides the `QueryBox` control with it's `QueryBoxViewModel`.

	My approach was to define a dependency property and then access it in the constructor of `QueryBox`:

	```csharp
    public QueryBox()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }

	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        name: nameof(ViewModel),
        propertyType: typeof(QueryBoxViewModel),
        ownerType: typeof(QueryBox)
    );
    public QueryBoxViewModel ViewModel
    {
        get => (QueryBoxViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }
	```

    This doesn't work, because `ViewModel` is always null.

-   My understanding is that I should do dependency injection in the constructor, but I do not know how to provide
    constructor parameters.

-   There is something like `<x:Arguments />` but it seems that this is no longer supported.

-   It's possible to assign to `DataContext` directly without any fancy work.

### Tasks

-   Try to get an event handler when the dependeny property changes.

-   Research how to inject a view model into another component.

### Theories

-   I suspect, that the constructor runs before it starts looking at the properties.

-   I suspect, that this is only possible to do from the view model.

    -   I suspect, that overriding `DataContext` messes things up.

### Result

-   It is possible to assign to the `DataContext` property directly from the parent:

    ```xaml
    <QueryBox DataContext="{Binding QueryBoxVM}" />
    ```
