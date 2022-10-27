using Bridge.Application.Places.Queries;
using Bridge.WebApp.Api.ApiClients;
using Bridge.WebApp.Pages.Home.Components;
using Bridge.WebApp.Pages.Home.Models;
using Bridge.WebApp.Services.Maps;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Bridge.WebApp.Pages.Home
{
    public partial class Index
    {

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
        /// 현위치 동향
        /// </summary>
        private double _easting = 5000;

        /// <summary>
        /// 현위치 북향
        /// </summary>
        private double _northing = 5000;

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

        protected override async Task OnInitializedAsync()
        {
            var geoResult =  await GeoService.GetLocationAsync();
            if (geoResult.Success)
            {
                var point = geoResult.Data!;
                _centerLocation = new LatLon(point.Latitude, point.Longitude);

                var addressResult = await ReverseGeocodeService.GetAddressAsync(point.Latitude, point.Longitude);
                _centerAddress = addressResult.Data;
            }
            else
            {
                Snackbar.Add(geoResult.Error, Severity.Error);
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
        }

        private async Task Settings_ClickAsync()
        {
            var parameters = new DialogParameters();
            parameters.Add(nameof(SearchSettingsDialog.Easting), _easting);
            parameters.Add(nameof(SearchSettingsDialog.Northing), _northing);
            parameters.Add(nameof(SearchSettingsDialog.Distance), _searchDistance);

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = DialogService.Show<SearchSettingsDialog>(string.Empty, parameters, options);
            var result = await dialog.Result;
            
            if (!result.Cancelled)
            {
                var resultData = (dynamic)result.Data;
                _easting = resultData.Easting;
                _northing = resultData.Northing;
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

    }
}
