using Bridge.WebApp.Pages.Home.Models;
using Bridge.WebApp.Services.DynamicMap;
using Bridge.WebApp.Services.DynamicMap.Naver;
using Bridge.WebApp.Services.ReverseGeocode;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bridge.WebApp.Pages.Home.ViewModels.Implement
{
    public class LocationSelectViewModel : ILocationSelectViewModel
    {
        private readonly IDynamicMapService _mapService;
        private readonly IReverseGeocodeService _reverseGeocodeService;

        public LocationSelectViewModel(IDynamicMapService mapService, IReverseGeocodeService reverseGeocodeService)
        {
            _mapService = mapService;
            _reverseGeocodeService = reverseGeocodeService;
        }

        public MudDialogInstance MudDialog { get; set; } = null!;
        public IHandleEvent Receiver { get; set; } = null!;

        public string MapElementId { get; } = "MapElement";

        public LatLon? CurrentLocation { get; set; }

        public string? CurrentAddress { get; set; }

        private async void OnContextMenuClicked(Tuple<string, MapPoint> tuple)
        {
            if(tuple.Item1 == "menu1")
            {
                CurrentLocation = new LatLon(tuple.Item2.Y, tuple.Item2.X);

                var result = await _reverseGeocodeService.GetAddressAsync(CurrentLocation.Latitude, CurrentLocation.Longitude);
                CurrentAddress = result.Data;
            }
          
        }

        public async Task Initialize()
        {
            _mapService.SetOnContextMenuClickedCallback(new(Receiver, OnContextMenuClicked));
            var mapOptions = new NaverMapService.MapOptions()
            {
                MapId = MapElementId,
                CenterX = CurrentLocation?.Longitude,
                CenterY = CurrentLocation?.Latitude,
            };

            await _mapService.InitAsync(mapOptions);
        }


        public async Task OnCancelClick()
        {
            MudDialog.Cancel();
            await Task.CompletedTask;
        }

        public async Task OnOkClick()
        {
            if (CurrentLocation == null) 
            {
                MudDialog.Cancel();
            }
            else
            {
                MudDialog.Close(DialogResult.Ok(
                    new
                    {
                        CurrentLocation,
                        CurrentAddress
                    }));
            }
            await Task.CompletedTask;
        }

      

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await _mapService.DisposeMapAsync();
            GC.SuppressFinalize(this);
        }
    }
}
