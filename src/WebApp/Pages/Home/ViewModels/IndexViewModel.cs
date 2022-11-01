using Bridge.Application.Places.Queries;
using Bridge.WebApp.Api.ApiClients;
using Bridge.WebApp.Pages.Home.Components;
using Bridge.WebApp.Pages.Home.Models;
using Bridge.WebApp.Services.DynamicMap;
using Bridge.WebApp.Services.DynamicMap.Naver;
using Bridge.WebApp.Services.GeoLocation;
using Bridge.WebApp.Services.ReverseGeocode;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;

namespace Bridge.WebApp.Pages.Home.ViewModels
{
    public class IndexViewModel : IIndexViewModel
    {
        private readonly string SESSION_ID = Guid.NewGuid().ToString();
        private readonly List<PlaceModel> _places = new();

        private readonly PlaceApiClient _placeApiClient;
        private readonly ISnackbar _snackbar;
        private readonly IDialogService _dialogService;
        private readonly IDynamicMapService _mapService;
        private readonly IHtmlGeoService _geoService;
        private readonly IReverseGeocodeService _reverseGeocodeService;
        private readonly IJSRuntime _jsRuntime;

        private IJSObjectReference? _jsModule;

        public IndexViewModel(PlaceApiClient placeApiClient, ISnackbar snackbar, IDialogService dialogService, IDynamicMapService mapService, IHtmlGeoService geoService, IReverseGeocodeService reverseGeocodeService, IJSRuntime jsRuntime)
        {
            _placeApiClient = placeApiClient;
            _snackbar = snackbar;
            _dialogService = dialogService;
            _mapService = mapService;
            _geoService = geoService;
            _reverseGeocodeService = reverseGeocodeService;
            _jsRuntime = jsRuntime;
        }

        public string MapElementId { get; } = "MapId";
        public bool Searched { get; private set; }
        public string SearchText { get; set; } = string.Empty;
        public LatLon? CurrentLocation { get; set; }
        public string? CurrentAddress { get; set; }
        public object? SelectedListItem { get; set; }
        public PlaceModel? SelectedPlace { get; set; }
        public IEnumerable<PlaceModel> Places => _places;
        public EventCallback SearchCompleted { get; set; }
        public IHandleEvent Receiver { get; set; } = null!;

        public async Task InitAsync()
        {
            _jsModule = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/Home/Index.razor.js");

            await GetCurrentLocationAsync();

            await Task.WhenAll(GetCurrentAddressAsync(), InitDynamicMapAsync());
        }

        private async Task GetCurrentLocationAsync()
        {
            var geoResult = await _geoService.GetLocationAsync();
            if (geoResult.Success)
            {
                var point = geoResult.Data!;
                CurrentLocation = new LatLon(point.Latitude, point.Longitude);
            }
            else
            {
                _snackbar.Add(geoResult.Error, Severity.Error);
            }
        }

        private async Task GetCurrentAddressAsync()
        {
            if (CurrentLocation == null)
            {
                CurrentAddress = "위치확인 불가";
                return;
            }

            var addressResult = await _reverseGeocodeService.GetAddressAsync(CurrentLocation.Latitude, CurrentLocation.Longitude);
            CurrentAddress = addressResult.Data;
        }

        private async Task InitDynamicMapAsync()
        {
            var mapOptions = new NaverMapService.MapOptions()
            {
                MapId = MapElementId,
                CenterX = CurrentLocation?.Longitude,
                CenterY = CurrentLocation?.Latitude
            };

            _mapService.SetOnSelectedMarkerChangedCallback(SESSION_ID, new(Receiver, OnSelectedMarkerChangedCallback));
            await _mapService.InitAsync(SESSION_ID, mapOptions);
        }

        private async void OnSelectedMarkerChangedCallback(string markerId)
        {
            if (long.TryParse(markerId, out long id))
            {
                var place = Places.FirstOrDefault(x => x.Id == id);
                SelectedPlace = place;
                SelectedListItem = place;

                if (_jsModule != null)
                    await _jsModule.InvokeVoidAsync("scrollTo", id.ToString());
            }
        }

        public async Task SearchPlacesAsync()
        {
            await LoadPlacesFromServer();

            await CreateMarkers();
        }

        private async Task LoadPlacesFromServer()
        {
            if (string.IsNullOrEmpty(SearchText)) 
                return;
            if (CurrentLocation == null) 
                return;

            var query = new SearchPlacesQuery()
            {
                SearchText = SearchText,
                Latitude = CurrentLocation.Latitude,
                Longitude = CurrentLocation.Longitude
            };
            var apiResult = await _placeApiClient.SearchPlaces(query);
            if (!apiResult.Success)
            {
                _snackbar.Add(apiResult.Error, Severity.Error);
                return;
            }
            if (apiResult.Data == null)
            {
                _snackbar.Add("데이터가 없습니다", Severity.Error);
                return;
            }

            _places.Clear();
            _places.AddRange(apiResult.Data.Select(x =>
            {
                var place = PlaceModel.Create(x);
                if (x.ImagePath != null)
                    place.ImageUrl = new Uri(_placeApiClient.HttpClient.BaseAddress!, x.ImagePath).ToString();
                return place;
            })
            .OrderBy(x => x.Distance));
        }

        private async Task CreateMarkers()
        {
            var markers = Places.Select(x => new Marker()
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Latitude = x.Latitude,
                Longitude = x.Longitude
            });
            await _mapService.ClearMarkersAsync(SESSION_ID);
            await _mapService.AddMarkersAsync(SESSION_ID, markers);

            if (SearchCompleted.HasDelegate)
                await SearchCompleted.InvokeAsync();
        }


        public async Task Handle_KeyUp(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                await SearchPlacesAsync();
            }
        }

        /// <summary>
        /// 장소 리스트 아이템을 선택할 때
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        public async Task Handle_PlaceSelected(PlaceModel place)
        {
            await _mapService.SelectMarkerAsync(SESSION_ID, place.Id.ToString());
            await _mapService.MoveAsync(SESSION_ID, place.Latitude, place.Longitude);
        }

        public async Task ShowLocationSelectionAsync()
        {
            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Medium,
                NoHeader = true
            };
            var dialogParameters = new DialogParameters
            {
                { nameof (LocationSelectionDialog.Longitude), CurrentLocation?.Longitude },
                { nameof (LocationSelectionDialog.Latitude), CurrentLocation?.Latitude },
                { nameof (LocationSelectionDialog.Address), CurrentAddress }
            };
            var dialog = _dialogService.Show<LocationSelectionDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var data = (dynamic)result.Data;
                CurrentLocation = new LatLon(data.Latitude, data.Longitude);
                CurrentAddress = data.Address;
            }
        }

        public async ValueTask DisposeAsync()
        {
            await _mapService.CloseAsync(SESSION_ID);
        }


    }
}
