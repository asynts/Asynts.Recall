using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Windows.Navigation;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Asynts.Recall.Frontend.Views;
using System;
using Microsoft.Extensions.DependencyInjection;
using Asynts.Recall.Backend.Services;
using Asynts.Recall.Backend.Persistance.Data;


namespace Asynts.Recall.Frontend.ViewModels
{
    public partial class PageViewModel : ObservableObject
    {
        public long DebugId { get; private set; }

        private readonly IRoutingService _routingService;

        public PageViewModel(ObjectIDGenerator idGenerator, IRoutingService routingService)
        {
            DebugId = idGenerator.GetId(this, out _);
            this._routingService = routingService;
        }

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
                // FIXME: Get a proper id here.
                PageId = 0,
            });
        }
    }
}
