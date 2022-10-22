using Bridge.Domain.Places.Entities;
using Bridge.Shared.Extensions;
using Bridge.WebApp.Api.ApiClients.Admin;
using Bridge.WebApp.Pages.Admin.Components;
using Bridge.WebApp.Pages.Admin.DataModels;
using Bridge.WebApp.Pages.Admin.Models;
using Bridge.WebApp.Services;
using Bridge.WebApp.Shared;
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
        public PlaceListModel Model { get; set; } = null!;

        [Inject]
        public AdminPlaceApiClient PlaceApiClient { get; set; } = null!;
        
        [Inject]
        public IExcelService ExcelService { get; set; } = null!;

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
            var result = await PlaceApiClient.GetPaginatedPlaceList(_placeType, _pageNumber, _rowsPerPage);
            if (!ValidationService.Validate(result))
                return;

            var placeList = result.Data!;

            _totalCount = placeList.TotalCount;
            _pageNumber = placeList.PageNumber;
            _pageCount = placeList.TotalPages;

            _places.Clear();
            _places.AddRange(placeList.List.Select(x => PlaceModel.ToPlaceModel(x)));
            StateHasChanged();
        }

        private async void Create_Click()
        {
            var parameters = new DialogParameters
            {
                { nameof(PlaceModalForm.FormMode), FormMode.Create }
            };

            var options = new DialogOptions { MaxWidth = MaxWidth.Large };
            var dialog = DialogService.Show<PlaceModalForm>(string.Empty, parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                await Load_ClickAsync();
            }
        }

        private async void CreateRestroom_Click()
        {
            var parameters = new DialogParameters
            {
                { nameof(RestroomModalForm.FormMode), FormMode.Create }
            };

            var options = new DialogOptions { MaxWidth = MaxWidth.Large };
            var dialog = DialogService.Show<RestroomModalForm>(string.Empty, parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                await Load_ClickAsync();
            }
        }
        
        private async void RestroomExcelDownload_Click()
        {
            await Model.DownloadExcel();
        }

        private async void RestroomExcelUpload_Click()
        {
            await Model.UploadExcel();

            if (!Model.UploadSuccessful)
            {
                Snackbar.Add(Model.UploadError);
                return;
            }

            Snackbar.Add($"{Model.BatchFailCount}/{Model.BatchTotalCount} 실패");
            foreach (var error in Model.BatchErrors)
            {
                Snackbar.Add(error);
                Console.WriteLine(error);
            }
        }

        private void ToggleShowOpeningTime_Click(PlaceModel place)
        {
            place.ShowOpeningTimes = !place.ShowOpeningTimes;
        }

        private async void EditPlace_Click(PlaceModel place)
        {
            if (place.Type == PlaceType.Restroom)
            {
                ShowRestroomModal(place);
                return;
            }
            
            var parameters = new DialogParameters
            {
                { nameof(PlaceModalForm.FormMode), FormMode.Update },
                { nameof(PlaceModalForm.PlaceId), place.Id }
            };

            var options = new DialogOptions { MaxWidth = MaxWidth.Large };
            var dialog = DialogService.Show<PlaceModalForm>(string.Empty, parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
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

        private async void ShowRestroomModal(PlaceModel place)
        {
            var parameters = new DialogParameters
            {
                { nameof(RestroomModalForm.FormMode), FormMode.Update },
                { nameof(RestroomModalForm.PlaceId), place.Id }
            };

            var options = new DialogOptions { MaxWidth = MaxWidth.Large };
            var dialog = DialogService.Show<RestroomModalForm>(string.Empty, parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
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

        private async void ClosePlace_Click(PlaceModel place)
        {
            var parameters = new DialogParameters
            {
                { nameof(ConfirmationDialog.Message), $"'{place.Name}' 을(를) 폐업하시겠습니까?"},
            };

            var options = new DialogOptions { MaxWidth = MaxWidth.Small };
            var dialog = DialogService.Show<ConfirmationDialog>(string.Empty, parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                var apiResult = await PlaceApiClient.ClosePlace(place.Id);
                if(ValidationService.Validate(apiResult))
                {
                    await Load_ClickAsync();
                }
            }
        }

        private async void PageNumberChanged(int pageNumber)
        {
            _pageNumber = pageNumber;
            await Load_ClickAsync();
        }

        private async void RowsPerPageChanged(int rowsPerPage)
        {
            _rowsPerPage = rowsPerPage;
            await Load_ClickAsync();
        }
    }
}
