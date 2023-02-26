### Notes

-	Somehow, I do not get a scrollbar in `<ContentControl>`.

-	Initially, I though that I had to add `<ScrollViewer>` to the template but that did not work:
	```xaml
	<DataTemplate DataType="{x:Type viewmodel:PageSearchViewModel}">
        <ScrollViewer>
            <local:PageSearch DataContext="{Binding}" />
        </ScrollViewer>
    </DataTemplate>
	```
    
-   When I added a `<ScrollViewer>` around the `<ContentControl>` that did not help.

-   When I replaced the `<StackPanel>` with a `<ItemsControl>` that did not help.

-   Adding a `<ScrollViewer>` around the `<ContentControl>` and replacing the `<StackPanel>` with a `<Grid>` resolved
    the issue.

### Theories

-   This may be caused by the `<StackPanel>` that the `<ContentControl>` sits in.
