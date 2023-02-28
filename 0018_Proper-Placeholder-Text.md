### Notes

-	I want proper placeholder text without any hacks.

-	My idea was to add a `PlaceholderAdorner` class which adds an attached property:

	```xaml
	<TextBox extensions:PlaceholderAdorner.Text="Placeholder..." />
	```

	In theory, I wrote all the code required for it but it doesn't seem to be valid XAML.

	This was based on this example but I made significant changes to it:
	https://stackoverflow.com/a/836463/8746648

-	This explains roughly what adorners are used for:
	https://stackoverflow.com/a/2611613/8746648

	-	I do feel like this is the correct tool here, since I need to measure the size of the element that I am
		decorating.

-	My understanding is that it's not possible to add an adorner from XAML.
	The trick might be to use an attached property and then add it from there.

-	For now, I decided to leave it as is since this isn't really important.

	-	If I do this

### Tasks

-	Research how attached properties work.

-	Read the blog post:
	http://www.ageektrapped.com/blog/the-missing-net-4-cue-banner-in-wpf-i-mean-watermark-in-wpf/
