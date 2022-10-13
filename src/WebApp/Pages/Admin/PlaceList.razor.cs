using Bridge.Domain.Places.Entities;
using Bridge.Shared.Extensions;
using Bridge.WebApp.Api.ApiClients;
using Bridge.WebApp.Models;
using Microsoft.AspNetCore.Components;

namespace Bridge.WebApp.Pages.Admin
{
    public partial class PlaceList
    {
        /// <summary>
        /// 장소 목록
        /// </summary>
        private readonly List<PlaceListModel> _places = new();

        /// <summary>
        /// 셀렉트 인풋에서 선택한 장소유형
        /// </summary>
        private PlaceType? _selectedPlaceType;

        /// <summary>
        /// 검색어
        /// </summary>
        private string _searchString = string.Empty;

        [Inject]
        public PlaceApiClient PlaceApiClient { get; set; } = null!;

        /// <summary>
        /// 입력된 검색어로 장소를 검색한다.
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        private bool Search(PlaceListModel place)
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;
            return place.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true ||
                place.Address.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true ||
                place.CategoriesString.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
        }

        private void Create_Click()
        {
            NavManager.NavigateTo(PageRoutes.Admin.PlaceCreate);
        }

        private async Task Load_ClickAsync()
        {
            if (!_selectedPlaceType.HasValue)
                return;

            var result = await PlaceApiClient.GetPlaceList(_selectedPlaceType.Value);
            if (!ValidationService.Validate(result))
                return;

            var placeList = result.Data!;
            _places.Clear();
            _places.AddRange(placeList.Select(x => PlaceListModel.ToPlaceModel(x)));
        }

        private void ToggleShowOpeningTime_Click(PlaceListModel place)
        {
            place.ShowOpeningTimes = !place.ShowOpeningTimes;
        }

        private void EditPlace_Click(PlaceListModel place)
        {
            var uri = PageRoutes.Admin.PlaceUpdate.AddRouteParam("PlaceId", place.Id);
            NavManager.NavigateTo(uri);
        }

        private void ManageProduct_Click(PlaceListModel place)
        {
            NavManager.NavigateTo(PageRoutes.Admin.PlaceProductList.AddRouteParam("PlaceId", place.Id));
        }
        

    }
}
