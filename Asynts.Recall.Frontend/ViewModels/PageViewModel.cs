using System.Collections.Generic;
using System.Runtime.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Asynts.Recall.Backend.Services;
using Asynts.Recall.Backend.Persistance.Data;
using Asynts.Recall.Backend.Persistance;

namespace Asynts.Recall.Frontend.ViewModels
{
    public partial class PageViewModel : ObservableObject
    {
        public long DebugId { get; private set; }

        private readonly IRoutingService _routingService;

        public PageViewModel(
            PageData pageData,
            ObjectIDGenerator idGenerator,
            IRoutingService routingService)
        {
            _routingService = routingService;

            Id = pageData.Id;
            Title = pageData.Title;
            Contents = pageData.Contents;
            Tags = pageData.Tags;

            DebugId = idGenerator.GetId(this, out _);
        }

        [ObservableProperty]
        private long id = 0;

        [ObservableProperty]
        private string title = "Title";

        [ObservableProperty]
        private string contents = "Contents";

        [ObservableProperty]
        private IList<string> tags = new List<string> { "tag_1", "tag_2", "tag_3" };

        [RelayCommand]
        public void ShowDetailsPage()
        {
            _routingService.Navigate(new PageDetailsRouteData
            {
                PageId = Id,
            });
        }
    }
}
