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

	-	I don't think that is relevant here.

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

-	My understanding is that `<MultiBinding>` allows multiple bindings to be combined with a converter.

-	Essentially, I need a converter that converts the `Text` into `Visiblity`.

-	This is a duplicate of this Stack Overflow question:
	https://stackoverflow.com/q/15176115/8746648

	-	Unfortunately, he did not find a solution except lifting up the state into the parent.

-	All that I need is to define a binding between the dependency property and the view model.

-	I tried using the following in the code-behind:
	```csharp
    public TextBox()
    {
        InitializeComponent();

        // ...

        SetBindingToViewModel("Text", TextProperty);
        SetBindingToViewModel("PlaceholderText", PlaceholderTextProperty);
    }

    private void SetBindingToViewModel(string viewModelPropertyName, DependencyProperty viewProperty)
    {
        var binding = new Binding();
        binding.Path = new PropertyPath(viewModelPropertyName);
        binding.Mode = BindingMode.TwoWay;
        binding.Source = DataContext;
        SetBinding(viewProperty, binding);
    }
	```
    And this worked for the `PlaceholderText`, but it doesn't seem that this worked for the `Text`.

-   Now that I think about it, I am not really certain what I am even trying to do.

    -   The value of the `Text` property lives in the `TextBoxViewModel`.

    -   There should be a two-way binding with the normal `<TextBox>`.
        Thus the text can be edited but it can also be edited from the view-model.

    -   Finally, I want to expose this text property like this: `<local:TextBox Text="Initial text" />`.

-   This talks about how to extend components in UWP:
    https://stackoverflow.com/q/47804109/8746648

    -   This suggests that it's possible to inherit from `<TextBox>` and then add one custom dependency property.

-   This is my exact question with `<TextBox>` watermark:
    https://putridparrot.com/blog/basics-of-extending-a-wpf-control/

### Tasks

-   Research how two-way bindings work conceptually.

-   Research how to properly "wrap" existing components.
    This describes what I want to do.

### Theories

-	Try to debug the binding.

-	I suspect, the solution is to lift up the state.
