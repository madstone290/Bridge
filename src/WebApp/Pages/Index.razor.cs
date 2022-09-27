using Bridge.WebApp.Api.ApiClients;
using Bridge.WebApp.Extensions;
using Bridge.WebApp.Models;
using Bridge.WebApp.Pages.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using static MudBlazor.Icons.Custom;

namespace Bridge.WebApp.Pages
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

        [Inject]
        public PlaceApiClient PlaceApiClient { get; set; } = null!;

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
            {
                return;
            }

            var result = await PlaceApiClient.GetPlacesByNameAndRegion(_searchText, 
                _easting - _searchDistance,
                _easting + _searchDistance, 
                _northing - _searchDistance,
                _northing + _searchDistance);

            if (!Snackbar.CheckSuccess(result))
                return;

            _placeList.AddRange(result.Data!.Select(x=>
            {
                var place = PlaceListModel.ToPlaceModel(x);
                place.CalcDistance(_easting, _northing);
                return place;
            }).OrderBy(x=> x.Distance));
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

    }
}
