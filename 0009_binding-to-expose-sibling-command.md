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

### Theories

-	I suspect, the binding is evaluated before the command is initialized properly.
