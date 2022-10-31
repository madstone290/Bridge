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
using MudBlazor;

namespace Bridge.WebApp.Pages.Home
{
    public partial class Index : IAsyncDisposable
    {
        private const string MapId = "map";
        private readonly string _mapSessionId = Guid.NewGuid().ToString();

        private object? _selectedPlace;

        private MudTextField<string>? _searchField;

        /// <summary>
        /// 검색어
        /// </summary>
        private string? _searchText;
        
        /// <summary>
        /// 검색된 장소 목록
        /// </summary>
        private List<PlaceListModel> _placeList = new();

        /// <summary>
        /// 검색 실행 여부. 실행 여부에 따라 빈 장소목록에 대한 출력이 다르다.
        /// </summary>
        private bool _searched;

        /// <summary>
        /// 검색 거리(m)
        /// </summary>
        private double _searchDistance = 5000;

        /// <summary>
        /// 검색 중심위치의 주소
        /// </summary>
        private string? _centerAddress;

        /// <summary>
        /// 검색 중심위치
        /// </summary>
        private LatLon? _centerLocation;

        [Inject]
        public PlaceApiClient PlaceApiClient { get; set; } = null!;

        [Inject]
        public IHtmlGeoService GeoService { get; set; } = null!;

        [Inject]
        public IReverseGeocodeService ReverseGeocodeService { get; set; } = null!;

        [Inject]
        public IDynamicMapService MapService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            var geoResult =  await GeoService.GetLocationAsync();
            if (geoResult.Success)
            {
                var point = geoResult.Data!;
                _centerLocation = new LatLon(point.Latitude, point.Longitude);
            }
            else
            {
                Snackbar.Add(geoResult.Error, Severity.Error);
            }

            var mapOptions = new NaverMapService.MapOptions()
            {
                MapId = MapId,
                CenterX = _centerLocation?.Longitude,
                CenterY = _centerLocation?.Latitude
            };
            MapService.SetOnSelectedMarkerChangedCallback(_mapSessionId, new(this, SelectedMarkerChanged));
            _ = MapService.InitAsync(_mapSessionId, mapOptions);

            if(_centerLocation != null)
            {
                var addressResult = await ReverseGeocodeService.GetAddressAsync(_centerLocation.Latitude, _centerLocation.Longitude);
                _centerAddress = addressResult.Data;
            }

            
        }

        private void SelectedMarkerChanged(string markerId)
        {
            if(long.TryParse(markerId, out long id))
            {
                var place = _placeList.FirstOrDefault(x => x.Id == id);
                _selectedPlace = place;
                StateHasChanged();
            }
        }

        public async Task AutoComplete_OnKeyUpAsync(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                await SearchPlacesAsync();
            }
        }

        private async Task SearchPlacesAsync()
        {
            if (_searchField == null)
                return;
            if (string.IsNullOrWhiteSpace(_searchText))
                return;
            if (_centerLocation == null)
                return;
            
            var query = new SearchPlacesQuery()
            {
                SearchText = _searchText,
                Latitude = _centerLocation.Latitude,
                Longitude = _centerLocation.Longitude
            };
            var result = await PlaceApiClient.SearchPlaces(query);

            if (!ValidationService.Validate(result))
                return;

            _placeList.Clear();
            _placeList.AddRange(result.Data!
                .Select(x => 
                {
                    var place = PlaceListModel.ToPlaceModel(x);
                    if (x.ImagePath != null)
                        place.ImageUrl = new Uri(PlaceApiClient.HttpClient.BaseAddress!, x.ImagePath).ToString();
                    return place;
                })
                .OrderBy(x => x.Distance));

            _searched = true;
            await _searchField.BlurAsync();

            #region 마커 표시
            var markers = _placeList.Select(x => new Marker()
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Latitude = x.Latitude,
                Longitude = x.Longitude
            });

            await MapService.ClearMarkersAsync(_mapSessionId);
            await MapService.AddMarkersAsync(_mapSessionId, markers);
            #endregion
        }

        private async Task Settings_ClickAsync()
        {
            var parameters = new DialogParameters();
            parameters.Add(nameof(SearchSettingsDialog.Distance), _searchDistance);

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = DialogService.Show<SearchSettingsDialog>(string.Empty, parameters, options);
            var result = await dialog.Result;
            
            if (!result.Cancelled)
            {
                var resultData = (dynamic)result.Data;
                _searchDistance = resultData.Distance;
            }
        }

        private async Task SelectLocation_ClickAsync()
        {
            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Medium,
                NoHeader = true
            };
            var dialogParameters = new DialogParameters
            {
                { nameof (LocationSelectionDialog.Longitude), _centerLocation?.Longitude },
                { nameof (LocationSelectionDialog.Latitude), _centerLocation?.Latitude },
                { nameof (LocationSelectionDialog.Address), _centerAddress }
            };
            var dialog = DialogService.Show<LocationSelectionDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var data = (dynamic)result.Data;
                _centerLocation = new LatLon(data.Latitude, data.Longitude);
                _centerAddress = data.Address;
            }
        }

        private void ListItem_Click(PlaceListModel place)
        {
            MapService.SelectMarkerAsync(_mapSessionId, place.Id.ToString());
            MapService.MoveAsync(_mapSessionId, place.Latitude, place.Longitude);
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await MapService.CloseAsync(_mapSessionId);
        }
    }
}
