using Bridge.Domain.Places.Entities;
using Bridge.Shared.Extensions;
using Bridge.WebApp.Api.ApiClients.Admin;
using Bridge.WebApp.Pages.Admin.Models;
using Microsoft.AspNetCore.Components;

namespace Bridge.WebApp.Pages.Admin
{
    public partial class PlaceList
    {
        /// <summary>
        /// 장소 목록
        /// </summary>
        private readonly List<PlaceModel> _places = new();

        /// <summary>
        ///  검색할 장소 타입
        /// </summary>
        private PlaceType? _placeType;

        /// <summary>
        /// 검색어
        /// </summary>
        private string _searchString = string.Empty;

        [Inject]
        public AdminPlaceApiClient PlaceApiClient { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await Load_ClickAsync();
        }

        /// <summary>
        /// 입력된 검색어로 장소를 검색한다.
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        private bool Search(PlaceModel place)
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
            var result = await PlaceApiClient.GetPlaceList(_placeType);
            if (!ValidationService.Validate(result))
                return;

            var placeList = result.Data!;
            _places.Clear();
            _places.AddRange(placeList.List.OrderByDescending(x=> x.CreationDateTime)
                .ThenByDescending(x=> x.Id)
                .Select(x => PlaceModel.ToPlaceModel(x)));
        }

        private void ToggleShowOpeningTime_Click(PlaceModel place)
        {
            place.ShowOpeningTimes = !place.ShowOpeningTimes;
        }

        private void EditPlace_Click(PlaceModel place)
        {
            var uri = PageRoutes.Admin.PlaceView.AddRouteParam("PlaceId", place.Id);
            NavManager.NavigateTo(uri);
        }

        private void ManageProduct_Click(PlaceModel place)
        {
            NavManager.NavigateTo(PageRoutes.Admin.PlaceProductList.AddRouteParam("PlaceId", place.Id));
        }
        

    }
}
