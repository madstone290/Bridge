using Bridge.Domain.Places.Entities;
using Bridge.Shared.Extensions;
using Bridge.WebApp.Api.ApiClients.Admin;
using Bridge.WebApp.Pages.Admin.Components;
using Bridge.WebApp.Pages.Admin.Models;
using Bridge.WebApp.Pages.Admin.Records;
using Bridge.WebApp.Services;
using Bridge.WebApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Bridge.WebApp.Pages.Admin
{
    public partial class PlaceList
    {
        private const string SelectAll = "전체";
        private readonly Dictionary<PlaceType, string> _placeTypeDiplayTexts = new()
        {
            { PlaceType.Other, "기타" },
            { PlaceType.Pharmacy, "약국" },
            { PlaceType.Hospital, "병원" },
            { PlaceType.Cafeteria, "카페" },
            { PlaceType.Restaurant, "식당" },
            { PlaceType.Restroom, "공중화장실" },
        };

        /// <summary>
        /// 장소 목록
        /// </summary>
        private readonly List<PlaceRecord> _places = new();

        /// <summary>
        ///  검색할 장소 타입
        /// </summary>
        private PlaceType? _placeType;

        /// <summary>
        /// 검색어
        /// </summary>
        private string _searchText = string.Empty;

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

        protected override void OnInitialized()
        {
            Load_ClickAsync();
        }

        private string GetPlaceTypeDisplayText(PlaceType? placeType)
        {
            if (placeType.HasValue)
                return _placeTypeDiplayTexts[placeType.Value];
            else
                return SelectAll;
        }

        private void SeachText_KeyUp(KeyboardEventArgs e)
        {
            if(e.Key == "Enter")
            {
                Load_ClickAsync();
            }
        }

        private async void Load_ClickAsync()
        {
            var result = await PlaceApiClient.GetPaginatedPlaceList(_searchText, _placeType, _pageNumber, _rowsPerPage);
            if (!ValidationService.Validate(result))
                return;

            var placeList = result.Data!;

            _totalCount = placeList.TotalCount;
            _pageNumber = placeList.PageNumber;
            _pageCount = placeList.TotalPages;

            _places.Clear();
            _places.AddRange(placeList.List.Select(x => PlaceRecord.ToPlaceModel(x)));
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
                Load_ClickAsync();
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
                Load_ClickAsync();
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

            var parameters = new DialogParameters
            {
                { nameof(BatchResultDialog.Total), Model.BatchTotalCount },
                { nameof(BatchResultDialog.Success), Model.BatchTotalCount - Model.BatchFailCount },
                { nameof(BatchResultDialog.Fail), Model.BatchFailCount },
                { nameof(BatchResultDialog.Errors), Model.BatchErrors },
            };

            var options = new DialogOptions { MaxWidth = MaxWidth.Large };
            var dialog = DialogService.Show<BatchResultDialog>(string.Empty, parameters, options);
            await dialog.Result;
        }

        private void ToggleShowOpeningTime_Click(PlaceRecord place)
        {
            place.ShowOpeningTimes = !place.ShowOpeningTimes;
        }

        private async void EditPlace_Click(PlaceRecord place)
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
                place.OpeningTimes = placeDto.OpeningTimes.Select(x => OpeningTimeRecord.Create(x));
                StateHasChanged();
            }
        }

        private async void ShowRestroomModal(PlaceRecord place)
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
                place.OpeningTimes = placeDto.OpeningTimes.Select(x => OpeningTimeRecord.Create(x));
                StateHasChanged();
            }
        }

        private void ManagePlace_Click(PlaceRecord place)
        {
            var uri = PageRoutes.Admin.PlaceView.AddRouteParam("PlaceId", place.Id);
            NavManager.NavigateTo(uri);
        }

        private void ManageProduct_Click(PlaceRecord place)
        {
            NavManager.NavigateTo(PageRoutes.Admin.PlaceProductList.AddRouteParam("PlaceId", place.Id));
        }

        private async void ClosePlace_Click(PlaceRecord place)
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
                    Load_ClickAsync();
                }
            }
        }

        private void PageNumberChanged(int pageNumber)
        {
            _pageNumber = pageNumber;
            Load_ClickAsync();
        }

        private void RowsPerPageChanged(int rowsPerPage)
        {
            _rowsPerPage = rowsPerPage;
            Load_ClickAsync();
        }
    }
}
