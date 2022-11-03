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
        /// <summary>
        /// 맵서비스 아이디
        /// </summary>
        private readonly string SESSION_ID = Guid.NewGuid().ToString();

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

        private async void OnLocationChanged(MapPoint point)
        {
            CurrentLocation = new LatLon(point.Y, point.X);

            var result = await _reverseGeocodeService.GetAddressAsync(point.Y, point.X);
            CurrentAddress = result.Data;
        }

        public async Task Initialize()
        {
            _mapService.SetOnClickCallback(SESSION_ID, new(Receiver, OnLocationChanged));
            var mapOptions = new NaverMapService.MapOptions()
            {
                MapId = MapElementId,
                CenterX = CurrentLocation?.Longitude,
                CenterY = CurrentLocation?.Latitude,
                ShowMyLocation = true
            };

            await _mapService.InitAsync(SESSION_ID, mapOptions);
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
                        CurrentLocation = CurrentLocation,
                        CurrentAddress = CurrentAddress
                    }));
            }
            await Task.CompletedTask;
        }

      

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await _mapService.CloseAsync(SESSION_ID);
            GC.SuppressFinalize(this);
        }
    }
}
