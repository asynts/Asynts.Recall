### Notes

-	Currently, the view model doesn't seem to be accessible by XAML.

-	This seems to be called "design time data":
	https://learn.microsoft.com/en-us/visualstudio/xaml-tools/xaml-designtime-data?view=vs-2022

-	Essentially, you just say `d:Whatever` next to your `Whatever` binding and that will be used in the designer instead.
	It is also possible to define resources that can be referenced for e.g. arrays:

	```xaml
    <!-- Typically, these namespaces would be defined on the top level 'UserControl', but here for simplicity: -->
    <Grid
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
        xmlns:sys="clr-namespace:System;assembly=mscorlib"
        mc:Ignorable="d">

        <Grid.Resources>
            <sys:String x:Key="DesignTimeTitle">Title</sys:String>
            <sys:String x:Key="DesignTimeContents">Contents</sys:String>
            <x:Array x:Key="DesignTimeTags" Type="sys:String">
                <sys:String>tag_1</sys:String>
                <sys:String>tag_2</sys:String>
                <sys:String>tag_3</sys:String>
            </x:Array>
        </Grid.Resources>

        <Label Content="{Binding Path=Title}" d:Content="{StaticResource DesignTimeTitle}" Grid.Row="0" Grid.Column="0" FontSize="16pt" />
        <local:TagList DataContext="{Binding Path=Tags}" d:DataContext="{StaticResource DesignTimeTags}" Grid.Row="1" Grid.Column="0" />
        <Label Content="{Binding Path=Contents}" d:Content="{StaticResource DesignTimeContents}" Grid.Row="2" Grid.Column="0" Background="Green" />
    </Grid>
	```

-   I was able to get this working by providing a default constructor in `PageViewModel`.

-   The problem is that in some instances I use `ActivatorUtilities.CreateInstance` which doesn't seem to like the default constructors that I am providing
    in the views.

    -   I created a `PageViewModelFactory` which is created using depencency injection instead.

-   I got another problem with the `PageViewModel`.
    Sometimes I want to show the details and sometimes I want to hide them:

    -   When viewing the designer of `Page` I want to see the details.

    -   When viewing the designer of `PageSearch` I want to hide the details and pass `ShowDetails=False` which is ignored.

-   Somehow it seems that `ShowDetails` defaults to `True` and thus I was able to simply remove the `d:Visibility` and use the default.
    That allowed the parent element to update it.

### Theories
