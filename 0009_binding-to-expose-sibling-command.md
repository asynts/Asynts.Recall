### Notes

-	I am trying to create a binding to expose a command from one view model to the view of another.

-	My goal is to be able to set the search bar to `#<id>` which will then trigger navigation.
	This is inspired by stack overflow and will become mandatory when I implement the tag tree properly.

-	I tried using a `DependencyProperty` to pass that command there:

	```xaml
	<local:PageSearch
		DataContext="{Binding}"
		ShowPageDetailsCommand="{Binding ElementName=refQueryBox, Path=ShowPageDetailsCommand, diag:PresentationTraceSources.TraceLevel=High}" />
	```

-	The binding did not work, this is the debugging information I got:

	```none
	System.Windows.Data Warning: 56 : Created BindingExpression (hash=5769005) for Binding (hash=63406242) BindingExpression:Path=ShowPageDetailsCommand; DataItem=null; 
	System.Windows.Data Warning: 58 :  Path: 'ShowPageDetailsCommand'
	System.Windows.Data Warning: 60 : BindingExpression (hash=5769005): Default mode resolved to OneWay
	System.Windows.Data Warning: 61 : BindingExpression (hash=5769005): Default update trigger resolved to PropertyChanged
	System.Windows.Data Warning: 62 : BindingExpression (hash=5769005): Attach to Asynts.Recall.Frontend.Views.PageSearch.ShowPageDetailsCommand (hash=33785274)
	System.Windows.Data Warning: 67 : BindingExpression (hash=5769005): Resolving source 
	System.Windows.Data Warning: 70 : BindingExpression (hash=5769005): Found data context element: <null> (OK)
	System.Windows.Data Warning: 74 :   Lookup name refQueryBox: queried PageSearch (hash=33785274)
	System.Windows.Data Warning: 74 :   Lookup name refQueryBox: queried ContentPresenter (hash=64636290)
	System.Windows.Data Warning: 74 :   Lookup name refQueryBox: queried ContentControl (hash=26602077)
	System.Windows.Data Warning: 78 : BindingExpression (hash=5769005): Activate with root item QueryBox (hash=32972388)
	System.Windows.Data Warning: 108 : BindingExpression (hash=5769005):   At level 0 - for QueryBox.ShowPageDetailsCommand found accessor <null>
	System.Windows.Data Error: 40 : BindingExpression path error: 'ShowPageDetailsCommand' property not found on 'object' ''QueryBox' (Name='refQueryBox')'. BindingExpression:Path=ShowPageDetailsCommand; DataItem='QueryBox' (Name='refQueryBox'); target element is 'PageSearch' (Name=''); target property is 'ShowPageDetailsCommand' (type 'RelayCommand')
	System.Windows.Data Warning: 80 : BindingExpression (hash=5769005): TransferValue - got raw value {DependencyProperty.UnsetValue}
	System.Windows.Data Warning: 88 : BindingExpression (hash=5769005): TransferValue - using fallback/default value <null>
	System.Windows.Data Warning: 89 : BindingExpression (hash=5769005): TransferValue - using final value <null>
	```

-	I tried using `Path=DataContext.ShowPageDetailsCommand` instead, that did not work either:

	```none
	System.Windows.Data Warning: 56 : Created BindingExpression (hash=5769005) for Binding (hash=63406242) BindingExpression:Path=DataContext.ShowPageDetailsCommand; DataItem=null; 
	System.Windows.Data Warning: 58 :  Path: 'DataContext.ShowPageDetailsCommand'
	System.Windows.Data Warning: 60 : BindingExpression (hash=5769005): Default mode resolved to OneWay
	System.Windows.Data Warning: 61 : BindingExpression (hash=5769005): Default update trigger resolved to PropertyChanged
	System.Windows.Data Warning: 62 : BindingExpression (hash=5769005): Attach to Asynts.Recall.Frontend.Views.PageSearch.ShowPageDetailsCommand (hash=33785274)
	System.Windows.Data Warning: 67 : BindingExpression (hash=5769005): Resolving source 
	System.Windows.Data Warning: 70 : BindingExpression (hash=5769005): Found data context element: <null> (OK)
	System.Windows.Data Warning: 74 :   Lookup name refQueryBox: queried PageSearch (hash=33785274)
	System.Windows.Data Warning: 74 :   Lookup name refQueryBox: queried ContentPresenter (hash=64636290)
	System.Windows.Data Warning: 74 :   Lookup name refQueryBox: queried ContentControl (hash=26602077)
	System.Windows.Data Warning: 78 : BindingExpression (hash=5769005): Activate with root item QueryBox (hash=32972388)
	System.Windows.Data Warning: 108 : BindingExpression (hash=5769005):   At level 0 - for QueryBox.DataContext found accessor DependencyProperty(DataContext)
	System.Windows.Data Warning: 104 : BindingExpression (hash=5769005): Replace item at level 0 with QueryBox (hash=32972388), using accessor DependencyProperty(DataContext)
	System.Windows.Data Warning: 101 : BindingExpression (hash=5769005): GetValue at level 0 from QueryBox (hash=32972388) using DependencyProperty(DataContext): QueryBoxViewModel (hash=55028720)
	System.Windows.Data Warning: 108 : BindingExpression (hash=5769005):   At level 1 - for QueryBoxViewModel.ShowPageDetailsCommand found accessor RuntimePropertyInfo(ShowPageDetailsCommand)
	System.Windows.Data Warning: 104 : BindingExpression (hash=5769005): Replace item at level 1 with QueryBoxViewModel (hash=55028720), using accessor RuntimePropertyInfo(ShowPageDetailsCommand)
	System.Windows.Data Warning: 101 : BindingExpression (hash=5769005): GetValue at level 1 from QueryBoxViewModel (hash=55028720) using RuntimePropertyInfo(ShowPageDetailsCommand): RelayCommand`1 (hash=9433441)
	System.Windows.Data Warning: 80 : BindingExpression (hash=5769005): TransferValue - got raw value RelayCommand`1 (hash=9433441)
	System.Windows.Data Warning: 84 : BindingExpression (hash=5769005): TransferValue - implicit converter produced <null>
	System.Windows.Data Warning: 89 : BindingExpression (hash=5769005): TransferValue - using final value <null>
	```

-	I tried updating the forwarding as follows:

	```xaml
	<local:Page
		DataContext="{Binding}"
		Margin="2px"
		ShowPageDetailsCommand="{Binding RelativeSource={RelativeSource AncestorType={x:Type UserControl}}, Path=ShowPageDetailsCommand}" />
	```

	That did not resolve the issue:

	```none
	System.Windows.Data Warning: 56 : Created BindingExpression (hash=64636290) for Binding (hash=63406242) BindingExpression:Path=DataContext.ShowPageDetailsCommand; DataItem=null; 
	System.Windows.Data Warning: 58 :  Path: 'DataContext.ShowPageDetailsCommand'
	System.Windows.Data Warning: 60 : BindingExpression (hash=64636290): Default mode resolved to OneWay
	System.Windows.Data Warning: 61 : BindingExpression (hash=64636290): Default update trigger resolved to PropertyChanged
	System.Windows.Data Warning: 62 : BindingExpression (hash=64636290): Attach to Asynts.Recall.Frontend.Views.PageSearch.ShowPageDetailsCommand (hash=33785274)
	System.Windows.Data Warning: 67 : BindingExpression (hash=64636290): Resolving source 
	System.Windows.Data Warning: 70 : BindingExpression (hash=64636290): Found data context element: <null> (OK)
	System.Windows.Data Warning: 74 :   Lookup name refQueryBox: queried PageSearch (hash=33785274)
	System.Windows.Data Warning: 74 :   Lookup name refQueryBox: queried ContentPresenter (hash=1048160)
	System.Windows.Data Warning: 74 :   Lookup name refQueryBox: queried ContentControl (hash=26602077)
	System.Windows.Data Warning: 78 : BindingExpression (hash=64636290): Activate with root item QueryBox (hash=32972388)
	System.Windows.Data Warning: 108 : BindingExpression (hash=64636290):   At level 0 - for QueryBox.DataContext found accessor DependencyProperty(DataContext)
	System.Windows.Data Warning: 104 : BindingExpression (hash=64636290): Replace item at level 0 with QueryBox (hash=32972388), using accessor DependencyProperty(DataContext)
	System.Windows.Data Warning: 101 : BindingExpression (hash=64636290): GetValue at level 0 from QueryBox (hash=32972388) using DependencyProperty(DataContext): QueryBoxViewModel (hash=55028720)
	System.Windows.Data Warning: 108 : BindingExpression (hash=64636290):   At level 1 - for QueryBoxViewModel.ShowPageDetailsCommand found accessor RuntimePropertyInfo(ShowPageDetailsCommand)
	System.Windows.Data Warning: 104 : BindingExpression (hash=64636290): Replace item at level 1 with QueryBoxViewModel (hash=55028720), using accessor RuntimePropertyInfo(ShowPageDetailsCommand)
	System.Windows.Data Warning: 101 : BindingExpression (hash=64636290): GetValue at level 1 from QueryBoxViewModel (hash=55028720) using RuntimePropertyInfo(ShowPageDetailsCommand): RelayCommand`1 (hash=25911262)
	System.Windows.Data Warning: 80 : BindingExpression (hash=64636290): TransferValue - got raw value RelayCommand`1 (hash=25911262)
	System.Windows.Data Warning: 84 : BindingExpression (hash=64636290): TransferValue - implicit converter produced <null>
	System.Windows.Data Warning: 89 : BindingExpression (hash=64636290): TransferValue - using final value <null>

	System.Windows.Data Warning: 56 : Created BindingExpression (hash=18492804) for Binding (hash=27440617) BindingExpression:Path=ShowPageDetailsCommand; DataItem=null; 
	System.Windows.Data Warning: 58 :  Path: 'ShowPageDetailsCommand'
	System.Windows.Data Warning: 60 : BindingExpression (hash=18492804): Default mode resolved to OneWay
	System.Windows.Data Warning: 61 : BindingExpression (hash=18492804): Default update trigger resolved to PropertyChanged
	System.Windows.Data Warning: 62 : BindingExpression (hash=18492804): Attach to Asynts.Recall.Frontend.Views.Page.ShowPageDetailsCommand (hash=654914)
	System.Windows.Data Warning: 66 : BindingExpression (hash=18492804): RelativeSource (FindAncestor) requires tree context
	System.Windows.Data Warning: 65 : BindingExpression (hash=18492804): Resolve source deferred
	System.Windows.Data Warning: 56 : Created BindingExpression (hash=2672115) for Binding (hash=27440617) BindingExpression:Path=ShowPageDetailsCommand; DataItem=null; 
	System.Windows.Data Warning: 58 :  Path: 'ShowPageDetailsCommand'
	System.Windows.Data Warning: 60 : BindingExpression (hash=2672115): Default mode resolved to OneWay
	System.Windows.Data Warning: 61 : BindingExpression (hash=2672115): Default update trigger resolved to PropertyChanged
	System.Windows.Data Warning: 62 : BindingExpression (hash=2672115): Attach to Asynts.Recall.Frontend.Views.Page.ShowPageDetailsCommand (hash=21522166)
	System.Windows.Data Warning: 66 : BindingExpression (hash=2672115): RelativeSource (FindAncestor) requires tree context
	System.Windows.Data Warning: 65 : BindingExpression (hash=2672115): Resolve source deferred
	System.Windows.Data Warning: 67 : BindingExpression (hash=18492804): Resolving source 
	System.Windows.Data Warning: 70 : BindingExpression (hash=18492804): Found data context element: <null> (OK)
	System.Windows.Data Warning: 73 :   Lookup ancestor of type UserControl: queried ContentPresenter (hash=48180537)
	System.Windows.Data Warning: 73 :   Lookup ancestor of type UserControl: queried StackPanel (hash=30971651)
	System.Windows.Data Warning: 73 :   Lookup ancestor of type UserControl: queried ItemsPresenter (hash=10309404)
	System.Windows.Data Warning: 73 :   Lookup ancestor of type UserControl: queried Border (hash=25675773)
	System.Windows.Data Warning: 73 :   Lookup ancestor of type UserControl: queried ItemsControl (hash=35632012)
	System.Windows.Data Warning: 73 :   Lookup ancestor of type UserControl: queried ContentPresenter (hash=29755367)
	System.Windows.Data Warning: 73 :   Lookup ancestor of type UserControl: queried Border (hash=66471715)
	System.Windows.Data Warning: 73 :   Lookup ancestor of type UserControl: queried PageSearch (hash=33785274)
	System.Windows.Data Warning: 72 :  RelativeSource.FindAncestor found PageSearch (hash=33785274)
	System.Windows.Data Warning: 78 : BindingExpression (hash=18492804): Activate with root item PageSearch (hash=33785274)
	System.Windows.Data Warning: 108 : BindingExpression (hash=18492804):   At level 0 - for PageSearch.ShowPageDetailsCommand found accessor DependencyProperty(ShowPageDetailsCommand)
	System.Windows.Data Warning: 104 : BindingExpression (hash=18492804): Replace item at level 0 with PageSearch (hash=33785274), using accessor DependencyProperty(ShowPageDetailsCommand)
	System.Windows.Data Warning: 101 : BindingExpression (hash=18492804): GetValue at level 0 from PageSearch (hash=33785274) using DependencyProperty(ShowPageDetailsCommand): <null>
	System.Windows.Data Warning: 80 : BindingExpression (hash=18492804): TransferValue - got raw value <null>
	System.Windows.Data Warning: 89 : BindingExpression (hash=18492804): TransferValue - using final value <null>
	System.Windows.Data Warning: 67 : BindingExpression (hash=2672115): Resolving source 
	System.Windows.Data Warning: 70 : BindingExpression (hash=2672115): Found data context element: <null> (OK)
	System.Windows.Data Warning: 73 :   Lookup ancestor of type UserControl: queried ContentPresenter (hash=47530006)
	System.Windows.Data Warning: 73 :   Lookup ancestor of type UserControl: queried StackPanel (hash=30971651)
	System.Windows.Data Warning: 73 :   Lookup ancestor of type UserControl: queried ItemsPresenter (hash=10309404)
	System.Windows.Data Warning: 73 :   Lookup ancestor of type UserControl: queried Border (hash=25675773)
	System.Windows.Data Warning: 73 :   Lookup ancestor of type UserControl: queried ItemsControl (hash=35632012)
	System.Windows.Data Warning: 73 :   Lookup ancestor of type UserControl: queried ContentPresenter (hash=29755367)
	System.Windows.Data Warning: 73 :   Lookup ancestor of type UserControl: queried Border (hash=66471715)
	System.Windows.Data Warning: 73 :   Lookup ancestor of type UserControl: queried PageSearch (hash=33785274)
	System.Windows.Data Warning: 72 :  RelativeSource.FindAncestor found PageSearch (hash=33785274)
	System.Windows.Data Warning: 78 : BindingExpression (hash=2672115): Activate with root item PageSearch (hash=33785274)
	System.Windows.Data Warning: 107 : BindingExpression (hash=2672115):   At level 0 using cached accessor for PageSearch.ShowPageDetailsCommand: DependencyProperty(ShowPageDetailsCommand)
	System.Windows.Data Warning: 104 : BindingExpression (hash=2672115): Replace item at level 0 with PageSearch (hash=33785274), using accessor DependencyProperty(ShowPageDetailsCommand)
	System.Windows.Data Warning: 101 : BindingExpression (hash=2672115): GetValue at level 0 from PageSearch (hash=33785274) using DependencyProperty(ShowPageDetailsCommand): <null>
	System.Windows.Data Warning: 80 : BindingExpression (hash=2672115): TransferValue - got raw value <null>
	System.Windows.Data Warning: 89 : BindingExpression (hash=2672115): TransferValue - using final value <null>
	```

	This all looks okay, but it resolves to `null` for some reason.

-	I tried updating the code to use `GetValue` and `SetValue` with no success:

	```csharp
	public RelayCommand<long>? ShowPageDetailsCommand
    {
        get => (RelayCommand<long>?)GetValue(ShowPageDetailsCommandProperty);
        set => SetValue(ShowPageDetailsCommandProperty, value);
    }
	```

-	I asked ChatGPT and it pointed out that I was using the wrong type when defining the command:

	```csharp
	public static readonly DependencyProperty ShowPageDetailsCommandProperty = DependencyProperty.Register(
        name: nameof(ShowPageDetailsCommand),
        propertyType: typeof(RelayCommand),
        ownerType: typeof(PageSearch)
    );
	```
	Should be:
	```csharp
	public static readonly DependencyProperty ShowPageDetailsCommandProperty = DependencyProperty.Register(
        name: nameof(ShowPageDetailsCommand),
        propertyType: typeof(RelayCommand<long>),
        ownerType: typeof(PageSearch)
    );
	```

	I don't think that I would have ever found that without ChatGPT!

### Theories

-	I suspect, the binding is evaluated before the command is initialized properly.

-	I suspect, that the forwarding in `PageSearch` does not work because we set the `DataContext`.
