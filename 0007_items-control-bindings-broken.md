### Notes

-	I got a `PageList` view that creates an `ItemControl`:

    ```xaml
	<ItemsControl ItemsSource="{Binding Pages}" Padding="2px">
        <ItemsControl.ItemTemplate>
            <DataTemplate>
                <local:Page DataContext="{Binding .}" Margin="2px" />
            </DataTemplate>
        </ItemsControl.ItemTemplate>
    </ItemsControl>
    ```

-   Somehow, `<local:Page>` uses the default data context assigned in it's constructor.

-   I set a breakpoint when we override the `DataContext` in `<local:Page>`.
    This revealed that the context is always `null`, when we write to it, thus we are not overriding the value.

-   I enabled diagnostics in XAML according to this page:
    https://wpf-tutorial.com/data-binding/debugging/
    ```xaml
    <local:Page DataContext="{Binding Path=., diag:PresentationTraceSources.TraceLevel=High}" Margin="2px" />
    ```
    This is the result I got:
    ```none
    System.Windows.Data Warning: 60 : BindingExpression (hash=34786562): Default mode resolved to OneWay
    System.Windows.Data Warning: 61 : BindingExpression (hash=34786562): Default update trigger resolved to PropertyChanged
    System.Windows.Data Warning: 62 : BindingExpression (hash=34786562): Attach to Asynts.Recall.Frontend.Views.Page.DataContext (hash=50198296)
    System.Windows.Data Warning: 67 : BindingExpression (hash=34786562): Resolving source 
    System.Windows.Data Warning: 70 : BindingExpression (hash=34786562): Found data context element: ContentPresenter (hash=44643603) (OK)
    System.Windows.Data Warning: 78 : BindingExpression (hash=34786562): Activate with root item PageData (hash=1267709177)
    System.Windows.Data Warning: 104 : BindingExpression (hash=34786562): Replace item at level 0 with PageData (hash=1267709177), using accessor {DependencyProperty.UnsetValue}
    System.Windows.Data Warning: 101 : BindingExpression (hash=34786562): GetValue at level 0 from PageData (hash=1267709177) using <null>: PageData (hash=1267709177)
    System.Windows.Data Warning: 80 : BindingExpression (hash=34786562): TransferValue - got raw value PageData (hash=1267709177)
    System.Windows.Data Warning: 89 : BindingExpression (hash=34786562): TransferValue - using final value PageData (hash=1267709177)
    ```

-   I updated `Pages` in `PageListViewModel` to be of type `ObservableCollection<PageViewModel>`:

    ```none
    System.Windows.Data Warning: 56 : Created BindingExpression (hash=36518691) for Binding (hash=12844374) BindingExpression:Path=.; DataItem=null; 
    System.Windows.Data Warning: 58 :  Path: '.'
    System.Windows.Data Warning: 60 : BindingExpression (hash=36518691): Default mode resolved to OneWay
    System.Windows.Data Warning: 61 : BindingExpression (hash=36518691): Default update trigger resolved to PropertyChanged
    System.Windows.Data Warning: 62 : BindingExpression (hash=36518691): Attach to Asynts.Recall.Frontend.Views.Page.DataContext (hash=61948991)
    System.Windows.Data Warning: 67 : BindingExpression (hash=36518691): Resolving source 
    System.Windows.Data Warning: 70 : BindingExpression (hash=36518691): Found data context element: ContentPresenter (hash=60232767) (OK)
    System.Windows.Data Warning: 78 : BindingExpression (hash=36518691): Activate with root item PageViewModel (hash=5223998)
    System.Windows.Data Warning: 104 : BindingExpression (hash=36518691): Replace item at level 0 with PageViewModel (hash=5223998), using accessor {DependencyProperty.UnsetValue}
    System.Windows.Data Warning: 101 : BindingExpression (hash=36518691): GetValue at level 0 from PageViewModel (hash=5223998) using <null>: PageViewModel (hash=5223998)
    System.Windows.Data Warning: 80 : BindingExpression (hash=36518691): TransferValue - got raw value PageViewModel (hash=5223998)
    System.Windows.Data Warning: 89 : BindingExpression (hash=36518691): TransferValue - using final value PageViewModel (hash=5223998)
    'Asynts.Recall.Frontend.exe' (CoreCLR: clrhost): Loaded 'C:\Program Files\dotnet\shared\Microsoft.WindowsDesktop.App\7.0.1\PresentationFramework-SystemXml.dll'. Skipped loading symbols. Module is optimized and the debugger option 'Just My Code' is enabled.
    System.Windows.Data Warning: 56 : Created BindingExpression (hash=3865173) for Binding (hash=12844374) BindingExpression:Path=.; DataItem=null; 
    System.Windows.Data Warning: 58 :  Path: '.'
    System.Windows.Data Warning: 60 : BindingExpression (hash=3865173): Default mode resolved to OneWay
    System.Windows.Data Warning: 61 : BindingExpression (hash=3865173): Default update trigger resolved to PropertyChanged
    System.Windows.Data Warning: 62 : BindingExpression (hash=3865173): Attach to Asynts.Recall.Frontend.Views.Page.DataContext (hash=20490669)
    System.Windows.Data Warning: 67 : BindingExpression (hash=3865173): Resolving source 
    System.Windows.Data Warning: 70 : BindingExpression (hash=3865173): Found data context element: ContentPresenter (hash=34786562) (OK)
    System.Windows.Data Warning: 78 : BindingExpression (hash=3865173): Activate with root item PageViewModel (hash=44643603)
    System.Windows.Data Warning: 104 : BindingExpression (hash=3865173): Replace item at level 0 with PageViewModel (hash=44643603), using accessor {DependencyProperty.UnsetValue}
    System.Windows.Data Warning: 101 : BindingExpression (hash=3865173): GetValue at level 0 from PageViewModel (hash=44643603) using <null>: PageViewModel (hash=44643603)
    System.Windows.Data Warning: 80 : BindingExpression (hash=3865173): TransferValue - got raw value PageViewModel (hash=44643603)
    System.Windows.Data Warning: 89 : BindingExpression (hash=3865173): TransferValue - using final value PageViewModel (hash=44643603)
    ```

-   It turns out, that I forgot the update the values of the `PageViewModel`, this is the new code:

    ```csharp
    var pageVM = _serviceProvider.GetRequiredService<PageViewModel>();
    pageVM.Title = page.Title;
    pageVM.Contents = page.Contents;
    pageVM.Tags = page.Tags;
    Pages.Add(pageVM);
    ```

-   Currently, it does compile but still doesn't work when I run it.
    What is weird is that I do get several errors in the IDE, all of them seem to be related to the CommunityToolkit.

-   I downgraded the toolkit from 8.1.0 to 8.0.0.
    The errors disappeared but this may just be a coincidence.

-   I removed the assignment in the constructor and this resolved the issue.
    However, now I don't understand what is going on.
    I tried this before and it did not work.

-   Even if I move the assignment above the call to `InitializeComponent()`, it still uses the default instead.

-   I verified again and `DataContext` is `null` when we assign to it in the constructor.

-   I tried removing the default value assignments in `PageViewModel`, but this didn't do anything.

-   I added a button in `Page` that would print out the current value, it's also the default value.

    -   I tried printing both `title` and `Title` and they both have the same value.

-   The constructor of `PageViewModel` runs four times.
    This is the expected value, since the view creates a default model and then overrides it with the new model.

-   I added some ids to the view models to help with debugging:
    ```none
    [PageListViewModel._searchEngine_ResultAvaliableEvent] adding page id=2
    [PageListViewModel._searchEngine_ResultAvaliableEvent] adding page id=3
    [Page.Page] vm.id=4
    [Page.Page] vm.id=5
    [PageViewModel.Print] id=5
    ```
    The last message is from the button press.
    This clearly shows that the default data context is used.

### Tasks

-   Print out the address of the data context being used.
    Maybe print out in the constructor?

-   If I inline the code from `Page.xaml`, does this solve the issue?

### Theories

-   I suspect, that this is a naming collision with the builtin `Page`.

-   I suspect, that assigning to the `DataContext` in XAML isn't possible if it's already set in the constructor.

    -   What speaks against this is that the assignment happens later.

    -   However, it could be that it doesn't want to override it unless it's already null.

-   I suspect, that when XAML sets the `DataContext` it doesn't notify the UI correctly and thus it keeps rendering the old version.

-   I suspect, that the rendering engine reads the values and connects the event listeners after that.
    If it is changed before the event handlers are registered, it would not notice.
