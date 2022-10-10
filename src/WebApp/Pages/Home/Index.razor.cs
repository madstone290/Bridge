using Bridge.Application.Places.Queries;
using Bridge.WebApp.Api.ApiClients;
using Bridge.WebApp.Extensions;
using Bridge.WebApp.Models;
using Bridge.WebApp.Pages.Home.Components;
using Bridge.WebApp.Services;
using Bridge.WebApp.Services.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.Drawing;

namespace Bridge.WebApp.Pages.Home
{
    public partial class Index
    {
        /// <summary>
        /// 검색어
        /// </summary>
        private string? _searchText;
        
        /// <summary>
        /// 검색된 장소 목록
        /// </summary>
        private List<PlaceListModel> _placeList = new();
        
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
        /// 인증 여부
        /// </summary>
        private bool _isAuthenticated;

        /// <summary>
        /// 검색 중심위치의 주소
        /// </summary>
        private string? _centerAddress;

        /// <summary>
        /// 검색 중심위치
        /// </summary>
        private GeoPoint? _centerLocation;

        [Inject]
        public PlaceApiClient PlaceApiClient { get; set; } = null!;

        [Inject]
        public IAuthService AuthService { get; set; } = null!;

        [Inject]
        public IHtmlGeoService GeoService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthService.GetAuthStateAsync();
            _isAuthenticated = authState.IsAuthenticated;

            GeoService.SuccessCallback = new EventCallback<GeoPoint>(this, ShowLocation);
            GeoService.ErrorCallback = new EventCallback<GeoError>(this, ShowError);
            await GeoService.GetLocationAsync();
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
            _placeList.Clear();

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

            if (!Snackbar.CheckSuccess(result))
                return;

            _placeList.AddRange(result.Data!
                .Select(x => PlaceListModel.ToPlaceModel(x))
                .OrderBy(x => x.Distance));
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

        private void ShowLocation(GeoPoint point)
        {
            _centerLocation = point;
            _centerAddress = $"{point?.Latitude:0.000000}, {point?.Longitude:0.000000}";
        }

        private void ShowError(GeoError error)
        {
            Console.WriteLine(error.Message);
        }
    }
}
