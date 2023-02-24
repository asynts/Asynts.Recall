### Notes

-   Currently, when a page is pressed we handle this in `Page.xaml.cs`:
    ```csharp
    private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs eventArgs)
    {
        ViewModel.ShowDetailsPage();
    }
    ```
    That's not optimal and in MVVM this should be done differently.

-	I tried this but I don't know how to continue:
	```xaml
    <Grid.Triggers>
        <EventTrigger RoutedEvent="MouseDown">
            <EventTrigger.Actions>
                    
            </EventTrigger.Actions>
        </EventTrigger>
    </Grid.Triggers>
    ```

-   I think the key is to understand how routed events work:
    https://learn.microsoft.com/en-us/dotnet/desktop/wpf/events/routed-events-overview?view=netdesktop-7.0

### Tasks

-   Research routed events:
    https://learn.microsoft.com/en-us/dotnet/desktop/wpf/events/routed-events-overview?view=netdesktop-7.0
