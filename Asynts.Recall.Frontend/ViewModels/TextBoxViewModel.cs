using Asynts.Recall.Frontend.Utility;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace Asynts.Recall.Frontend.ViewModels;

public partial class TextBoxViewModel : ObservableObject
{
    // For designer only.
    public TextBoxViewModel()
    {
        text = "";
        placeholderText = "Placeholder...";
    }

    public TextBoxViewModel(RuntimeModeMarker runtimeModeMarker)
    {
        text = "";
        placeholderText = "";
    }

    public Visibility PlaceholderVisibility
    {
        get
        {
            if (Text.Length == 0)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Hidden;
            }
        }
    }

    public string text;
    public string Text
    {
        get
        {
            return text;
        }
        set
        {
            OnPropertyChanging(nameof(PlaceholderVisibility));
            OnPropertyChanging();
            text = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(PlaceholderVisibility));
        }
    }

    [ObservableProperty]
    public string placeholderText;
}
