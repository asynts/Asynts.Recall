using Asynts.Recall.Frontend.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Asynts.Recall.Frontend.Views
{
    /// <summary>
    /// Interaction logic for TextBox.xaml
    /// </summary>
    public partial class TextBox : UserControl
    {
        public TextBox()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = new TextBoxViewModel();
            }
            else
            {
                DataContext = App.Current.Services.GetRequiredService<TextBoxViewModel>();
            }

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

        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }
        public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(
            name: nameof(PlaceholderText),
            propertyType: typeof(string),
            ownerType: typeof(TextBox)
        );

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            name: nameof(Text),
            propertyType: typeof(string),
            ownerType: typeof(TextBox)
        );
    }
}
