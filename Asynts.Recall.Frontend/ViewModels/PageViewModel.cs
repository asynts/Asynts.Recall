using System.Collections.Generic;
using System.Runtime.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Asynts.Recall.Backend.Services;
using Asynts.Recall.Backend.Persistance.Data;
using Asynts.Recall.Backend.Persistance;
using System.Windows;
using System;

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

            uuid = "e6e100e4-0f27-405d-87bf-d4852b675769";
            title = "Title";
            summary = "Summary";
            details = "Details";
            tags = new List<string> { "tag/1/", "tag/2/", "tag/3/" };
        }

        // For designer only
        public PageViewModel(PageData pageData)
        {
            _routingService = null!;

            uuid = pageData.Uuid;
            title = pageData.Title;
            summary = pageData.Summary;
            tags = pageData.Tags;
            details = pageData.Details;
        }

        public PageViewModel(
            PageData pageData,
            IRoutingService routingService)
        {
            _routingService = routingService;

            uuid = pageData.Uuid;
            title = pageData.Title;
            summary = pageData.Summary;
            tags = pageData.Tags;
            details = pageData.Details;
        }

        [ObservableProperty]
        private string uuid;

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string summary;

        [ObservableProperty]
        private string? details;

        [ObservableProperty]
        private IList<string> tags;

        [RelayCommand]
        public void ShowDetailsPage()
        {
            _routingService.Navigate(new PageDetailsRouteData
            {
                PageUuid = Uuid,
            });
        }
    }
}
