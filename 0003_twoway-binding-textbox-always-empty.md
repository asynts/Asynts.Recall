### Notes

-	I was able to verify that it works if I remove the focus from the `TextBox` before hitting RETURN.

-	This StackOverflow answer suggests that this is some weird behaviour with WPF:
	https://stackoverflow.com/a/22253816/8746648

### Theories

-	I suspect that this is because `Text` isn't updated until I defocus it.
