### Notes

-	I want to expose my `Text` dependency property on `TextBox` to the `TextBoxViewModel`.

-	To achieve this, I want to create a two-way binding but I don't know the syntax for that.

-	The question isn't how to define that binding, that's trivial:
	```xaml
	<Foo Text={Binding Path=Text, Mode=TwoWay} />
	```
	The real question is, where to define that binding.

-	This talks about `<MultiBinding>` but I didn't read it completely:
	https://stackoverflow.com/a/410681/8746648

-	I got some issues with the source generation again, I need to delete the `obj` and `bin` folders all the time.

-	I started up Visual Studio and it detected a "cycle" and was unable to load the `Asynts.Recall.Backend` project.
	Restarting seems to help.

-	I tried using this to bind the property to the view model but that doesn't work.
	The view model `Text` isn't updated:

	```xaml
	<UserControl.Resources>
        <Style TargetType="{x:Type local:TextBox}">
            <Setter Property="Text" Value="{Binding Path=Text, Mode=TwoWay}" />
        </Style>
    </UserControl.Resources>
	```

-	I tried this but it doesn't work either:
	```xaml
	<UserControl.Style>
        <Style TargetType="{x:Type local:TextBox}">
            <Setter Property="Text" Value="{Binding Path=Text, Mode=TwoWay}" />
            <d:Setter Property="PlaceholderText" Value="Placeholder..." />
        </Style>
    </UserControl.Style>
	```

### Tasks

-	Read about style in XAML:
	https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/styles-templates-overview?view=netdesktop-7.0

### Theories

-	I suspect, that the style can be used to define this.

-	I suspect, that `<MultiBinding>` is used to make this work.

-	Try to debug the binding.
