using Bridge.WebApp.Api.ApiClients;
using Bridge.WebApp.Extensions;
using Bridge.WebApp.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

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
        private double _centerEasting = 5000;

        /// <summary>
        /// 현위치 북향
        /// </summary>
        private double _centerNorthing = 5000;

        /// <summary>
        /// 동향 검색 범위(m). 좌우로 각각 적용된다.
        /// ex) 5000: 중심위치에서 좌5km, 우5km
        /// </summary>
        private double _eastingSearchRange = 4000;

        /// <summary>
        /// 북향 검색 범위(m). 상하로 각각 적용된다.
        /// ex) 5000: 중심위치에서 상5km, 하5km
        /// </summary>
        private double _northingSearchRange = 4000;

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
                _centerEasting - _eastingSearchRange,
                _centerEasting + _eastingSearchRange, 
                _centerNorthing - _northingSearchRange,
                _centerNorthing + _northingSearchRange);

            if (!Snackbar.CheckSuccess(result))
                return;

            _placeList.AddRange(result.Data!.Select(x=>
            {
                var place = PlaceListModel.ToPlaceModel(x);
                place.CalcDistance(_centerEasting, _centerNorthing);
                return place;
            }).OrderBy(x=> x.Distance));
        }

    }
}
