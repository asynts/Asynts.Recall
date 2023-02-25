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

-   It seems that it's not trivial to connect a command to an event:
    https://stackoverflow.com/a/7756809/8746648

-   This explains how to set this up properly with the new "behaviours":
    https://stackoverflow.com/a/61547718/8746648

### Tasks

-   Research routed events:
    https://learn.microsoft.com/en-us/dotnet/desktop/wpf/events/routed-events-overview?view=netdesktop-7.0

### Result

-   I am using `xmlns:i="http://schemas.microsoft.com/xaml/behaviors"` with the following:
    ```xaml
    <i:Interaction.Triggers>
        <i:EventTrigger EventName="MouseDown">
            <i:InvokeCommandAction Command="{Binding ShowDetailsPageCommand}" />
        </i:EventTrigger>    
    </i:Interaction.Triggers>
    ```
