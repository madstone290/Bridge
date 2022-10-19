using Bridge.Domain.Places.Entities;
using Bridge.Shared.Extensions;
using Bridge.WebApp.Api.ApiClients.Admin;
using Bridge.WebApp.Pages.Admin.Components;
using Bridge.WebApp.Pages.Admin.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

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

        private int _totalCount;
        private int _pageCount;
        private int _pageNumber = 1;
        private int _rowsPerPage = 10;

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

        private async Task Load_ClickAsync()
        {
            var result = await PlaceApiClient.GetPlaceList(_placeType, _pageNumber, _rowsPerPage);
            if (!ValidationService.Validate(result))
                return;

            var placeList = result.Data!;

            _totalCount = placeList.TotalCount;
            _pageNumber = placeList.PageNumber;
            _pageCount = placeList.TotalPages;

            _places.Clear();
            _places.AddRange(placeList.List.Select(x => PlaceModel.ToPlaceModel(x)));
        }

        private async void Create_Click()
        {
            var parameters = new DialogParameters
            {
                { nameof(PlaceModalForm.FormMode), FormMode.Create }
            };

            var options = new DialogOptions { MaxWidth = MaxWidth.Large};
            var dialog = DialogService.Show<PlaceModalForm>(string.Empty, parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                await Load_ClickAsync();
                StateHasChanged();
            }
        }

        private void ToggleShowOpeningTime_Click(PlaceModel place)
        {
            place.ShowOpeningTimes = !place.ShowOpeningTimes;
        }

        private async void EditPlace_Click(PlaceModel place)
        {
            var parameters = new DialogParameters
            {
                { nameof(PlaceModalForm.FormMode), FormMode.Update },
                { nameof(PlaceModalForm.PlaceId), place.Id }
            };

            var options = new DialogOptions { MaxWidth = MaxWidth.Large };
            var dialog = DialogService.Show<PlaceModalForm>(string.Empty, parameters, options);
            var dialogResult = await dialog.Result;
            if(!dialogResult.Cancelled)
            {
                var placeId = (long)dialogResult.Data;
                var placeResult = await PlaceApiClient.GetPlaceById(placeId);
                if (!ValidationService.Validate(placeResult))
                    return;

                var placeDto = placeResult.Data!;
                place.Type = placeDto.Type;
                place.Name = placeDto.Name;
                place.BaseAddress = placeDto.Address.BaseAddress;
                place.DetailAddress = placeDto.Address.DetailAddress;
                place.Categories = placeDto.Categories;
                place.ContactNumber = placeDto.ContactNumber;
                place.OpeningTimes = placeDto.OpeningTimes.Select(x => OpeningTimeModel.Create(x));
                StateHasChanged();
            }
        }

        private void ManagePlace_Click(PlaceModel place)
        {
            var uri = PageRoutes.Admin.PlaceView.AddRouteParam("PlaceId", place.Id);
            NavManager.NavigateTo(uri);
        }

        private void ManageProduct_Click(PlaceModel place)
        {
            NavManager.NavigateTo(PageRoutes.Admin.PlaceProductList.AddRouteParam("PlaceId", place.Id));
        }
        
        private async void PageNumberChanged(int pageNumber)
        {
            _pageNumber = pageNumber;
            await Load_ClickAsync();
            StateHasChanged();
        }

        private async void RowsPerPageChanged(int rowsPerPage)
        {
            _rowsPerPage = rowsPerPage;
            await Load_ClickAsync();
            StateHasChanged();
        }
    }
}
