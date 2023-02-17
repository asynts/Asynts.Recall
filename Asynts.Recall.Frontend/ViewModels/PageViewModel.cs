using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Windows.Navigation;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Asynts.Recall.Frontend.Views;

namespace Asynts.Recall.Frontend.ViewModels
{
    public partial class PageViewModel : ObservableObject
    {
        public long Id { get; private set; }

        public PageViewModel(ObjectIDGenerator idGenerator)
        {
            Id = idGenerator.GetId(this, out _);
        }

        [ObservableProperty]
        private string title = "Title";

        [ObservableProperty]
        private string contents = "Contents";

        [ObservableProperty]
        private IList<string> tags = new List<string> { "tag_1", "tag_2", "tag_3" };

        [RelayCommand]
        public void NavigateDetailsPage(NavigationService navigationService)
        {
            navigationService.Navigate(new PageDetails());
        }
    }
}
