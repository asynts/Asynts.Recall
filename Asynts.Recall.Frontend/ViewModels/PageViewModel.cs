using System.Collections.Generic;
using System.Runtime.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Asynts.Recall.Backend.Services;
using Asynts.Recall.Backend.Persistance.Data;
using Asynts.Recall.Backend.Persistance;
using System.Windows;

namespace Asynts.Recall.Frontend.ViewModels
{
    public class PageViewModelFactory
    {
        private readonly IRoutingService _routingService;

        public PageViewModelFactory(IRoutingService routingService)
        {
            this._routingService = routingService;
        }

        public PageViewModel Create(PageData pageData)
        {
            return new PageViewModel(
                routingService: _routingService,
                pageData: pageData
            );
        }
    }

    public partial class PageViewModel : ObservableObject
    {
        private readonly IRoutingService _routingService;

        // For designer only.
        public PageViewModel()
        {
            _routingService = null!;

            id = 0;
            title = "Title";
            contents = "Contents";
            details = "Details";
            tags = new List<string> { "tag/1/", "tag/2/", "tag/3/" };
        }

        // For designer only
        public PageViewModel(PageData pageData)
        {
            _routingService = null!;

            id = pageData.Id;
            title = pageData.Title;
            contents = pageData.Contents;
            tags = pageData.Tags;
            details = pageData.Details;
        }

        public PageViewModel(
            PageData pageData,
            IRoutingService routingService)
        {
            _routingService = routingService;

            id = pageData.Id;
            title = pageData.Title;
            contents = pageData.Contents;
            tags = pageData.Tags;
            details = pageData.Details;
        }

        [ObservableProperty]
        private long id;

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string contents;

        [ObservableProperty]
        private string? details;

        [ObservableProperty]
        private IList<string> tags;

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
