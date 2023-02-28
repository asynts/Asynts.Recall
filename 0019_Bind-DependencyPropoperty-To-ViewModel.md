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

### Tasks

-	Read about style in XAML:
	https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/styles-templates-overview?view=netdesktop-7.0

### Theories

-	I suspect, that the style can be used to define this.

-	I suspect, that `<MultiBinding>` is used to make this work.
