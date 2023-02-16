using System.Collections.Generic;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Asynts.Recall.Frontend.ViewModels
{
    public partial class PageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string title = "Title";

        [ObservableProperty]
        private string contents = "Contents";

        [ObservableProperty]
        private IList<string> tags = new List<string> { "tag_1", "tag_2", "tag_3" };
    }
}
