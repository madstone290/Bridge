using Bridge.Domain.Places.Entities;
using Bridge.Shared.Extensions;
using Bridge.WebApp.Api.ApiClients.Admin;
using Bridge.WebApp.Pages.Admin.Components;
using Bridge.WebApp.Pages.Admin.Records;
using Bridge.WebApp.Services;
using Bridge.WebApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Bridge.WebApp.Pages.Admin.ViewModels
{
    public class PlaceListViewModel : IPlaceListViewModel
    {
        private const string SelectAll = "전체";
        private readonly Dictionary<PlaceType, string> _placeTypeTexts = new()
        {
            { Domain.Places.Entities.PlaceType.Other, "기타" },
            { Domain.Places.Entities.PlaceType.Pharmacy, "약국" },
            { Domain.Places.Entities.PlaceType.Hospital, "병원" },
            { Domain.Places.Entities.PlaceType.Cafeteria, "카페" },
            { Domain.Places.Entities.PlaceType.Restaurant, "식당" },
            { Domain.Places.Entities.PlaceType.Restroom, "공중화장실" },
        };
        private readonly List<PlaceRecord> _placeRecords = new();
        private readonly ExcelOptions _excelOptions = new()
        {
            Columns = typeof(DaeguRestroomExcelRecord).GetProperties()
               .Select(x => new ExcelOptions.Column(x.Name, x.GetCustomAttribute<DisplayAttribute>()?.Name ?? x.Name))
        };

        private readonly AdminRestroomApiClient _restroomApiClient;
        private readonly AdminPlaceApiClient _placeApiClient;
        private readonly IExcelService _excelService;
        private readonly IApiResultValidationService _validationService;
        private readonly IDialogService _dialogService;
        private readonly NavigationManager _navManager;

        public PlaceListViewModel(AdminRestroomApiClient restroomApiClient, AdminPlaceApiClient placeApiClient, IExcelService excelService, IApiResultValidationService validationService, IDialogService dialogService, NavigationManager navManager)
        {
            _restroomApiClient = restroomApiClient;
            _placeApiClient = placeApiClient;
            _excelService = excelService;
            _validationService = validationService;
            _dialogService = dialogService;
            _navManager = navManager;
        }


        /// <summary>
        ///  검색할 장소 타입
        /// </summary>
        public PlaceType? PlaceType { get; set; }
        public string SearchText { get; set; } = string.Empty;
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; } = 1;
        public int RowsPerPage { get; set; } = 10;
        public IEnumerable<PlaceRecord> Places => _placeRecords;

        private async Task LoadPlaces()
        {
            var result = await _placeApiClient.GetPaginatedPlaceList(SearchText, PlaceType, PageNumber, RowsPerPage);
            if (!_validationService.Validate(result))
                return;

            var placeList = result.Data!;

            TotalCount = placeList.TotalCount;
            PageNumber = placeList.PageNumber;
            PageCount = placeList.TotalPages;

            _placeRecords.Clear();
            _placeRecords.AddRange(placeList.List.Select(x => PlaceRecord.ToPlaceModel(x)));
        }

        public Task Initialize()
        {
            return Task.CompletedTask;
        }

        public string GetPlaceTypeText(PlaceType? placeType)
        {
            if (placeType.HasValue)
                return _placeTypeTexts[placeType.Value];
            else
                return SelectAll;
        }

        public async Task OnSearchTextKeyUp(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await LoadPlaces();
            }
        }
        
        public async Task OnLoadClick()
        {
            await LoadPlaces();
        }

        public async Task OnCreateClick()
        {
            var parameters = new DialogParameters
            {
                { nameof(PlaceModalForm.FormMode), FormMode.Create }
            };

            var options = new DialogOptions { MaxWidth = MaxWidth.Large };
            var dialog = _dialogService.Show<PlaceModalForm>(string.Empty, parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                await LoadPlaces();
            }
        }

        public async Task OnCreateRestroomClick()
        {
            var parameters = new DialogParameters
            {
                { nameof(RestroomModalForm.FormMode), FormMode.Create }
            };

            var options = new DialogOptions { MaxWidth = MaxWidth.Large };
            var dialog = _dialogService.Show<RestroomModalForm>(string.Empty, parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                await LoadPlaces();
            }
        }

        public async Task OnRestroomExcelDownloadClick()
        {
            await _excelService.DownloadAsync<DaeguRestroomExcelRecord>("화장실 폼.xlsx", new List<DaeguRestroomExcelRecord>(), _excelOptions);
        }

        public async Task OnRestroomExcelUploadClick()
        {
            // 엑셀에서 데이터 읽기
            var restrooms = await _excelService.UploadAsync<DaeguRestroomExcelRecord>(_excelOptions);
            foreach (var restroom in restrooms)
                restroom.ReadFromText();

            if (!restrooms.Any())
                return;

            // 데이터 저장
            var command = new Application.Places.Commands.CreateRestroomBatchCommand()
            {
                Commands = restrooms.Select(x => new Application.Places.Commands.CreateRestroomCommand()
                {
                    Name = x.Name,
                    Address = new Application.Places.Dtos.AddressDto()
                    {
                        BaseAddress = x.BaseAddress,
                        DetailAddress = x.DetailAddress
                    },
                    IsUnisex = x.IsUnisex,
                    DiaperTableLocation = x.DiaperTableLocation,
                    MaleToilet = x.MaleToilet,
                    MaleUrinal = x.MaleUrinal,
                    MaleDisabledToilet = x.MaleDisabledToilet,
                    MaleDisabledUrinal = x.MaleDisabledUrinal,
                    MaleKidToilet = x.MaleKidToilet,
                    MaleKidUrinal = x.MaleKidUrinal,
                    FemaleToilet = x.FemaleToilet,
                    FemaleDisabledToilet = x.FemaleDisabledToilet,
                    FemaleKidToilet = x.FemaleKidToilet,
                    LastUpdateDateTimeLocal = x.LastUpdateDateTime,
                    OpeningTimes = new List<Application.Places.Dtos.OpeningTimeDto>()
                    {
                        new Application.Places.Dtos.OpeningTimeDto(){ Day = DayOfWeek.Monday, TwentyFourHours = x.Is24Hours, OpenTime = x.OpenTime, CloseTime = x.CloseTime },
                        new Application.Places.Dtos.OpeningTimeDto(){ Day = DayOfWeek.Tuesday, TwentyFourHours = x.Is24Hours, OpenTime = x.OpenTime, CloseTime = x.CloseTime },
                        new Application.Places.Dtos.OpeningTimeDto(){ Day = DayOfWeek.Wednesday, TwentyFourHours = x.Is24Hours, OpenTime = x.OpenTime, CloseTime = x.CloseTime },
                        new Application.Places.Dtos.OpeningTimeDto(){ Day = DayOfWeek.Thursday, TwentyFourHours = x.Is24Hours, OpenTime = x.OpenTime, CloseTime = x.CloseTime },
                        new Application.Places.Dtos.OpeningTimeDto(){ Day = DayOfWeek.Friday, TwentyFourHours = x.Is24Hours, OpenTime = x.OpenTime, CloseTime = x.CloseTime },
                        new Application.Places.Dtos.OpeningTimeDto(){ Day = DayOfWeek.Saturday, TwentyFourHours = x.Is24Hours, OpenTime = x.OpenTime, CloseTime = x.CloseTime },
                        new Application.Places.Dtos.OpeningTimeDto(){ Day = DayOfWeek.Sunday, TwentyFourHours = x.Is24Hours, OpenTime = x.OpenTime, CloseTime = x.CloseTime },
                    }
                })
            };

            var result = await _restroomApiClient.CreateRestroomBatch(command);
            _validationService.Validate(result);

            // 저장 결과 출력
            var parameters = new DialogParameters
            {
                { nameof(BatchResultDialog.Total), restrooms.Count() },
                { nameof(BatchResultDialog.Success), restrooms.Count()  - result.Data!.Count },
                { nameof(BatchResultDialog.Fail),  result.Data!.Count },
                { nameof(BatchResultDialog.Errors), result.Data },
            };

            var options = new DialogOptions { MaxWidth = MaxWidth.Large };
            var dialog = _dialogService.Show<BatchResultDialog>(string.Empty, parameters, options);
            await dialog.Result;
        }

        public async Task OnShowOpeningTimeClick(PlaceRecord place)
        {
            place.ShowOpeningTimes = !place.ShowOpeningTimes;
            await Task.CompletedTask;
        }

        public async Task OnEditPlaceClick(PlaceRecord place)
        {
            if (place.Type == Domain.Places.Entities.PlaceType.Restroom)
            {
                await ShowRestroomModal(place);
                return;
            }

            var parameters = new DialogParameters
            {
                { nameof(PlaceModalForm.FormMode), FormMode.Update },
                { nameof(PlaceModalForm.PlaceId), place.Id }
            };

            var options = new DialogOptions { MaxWidth = MaxWidth.Large };
            var dialog = _dialogService.Show<PlaceModalForm>(string.Empty, parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                var placeId = (long)dialogResult.Data;
                var placeResult = await _placeApiClient.GetPlaceById(placeId);
                if (!_validationService.Validate(placeResult))
                    return;

                var placeDto = placeResult.Data!;
                place.Type = placeDto.Type;
                place.Name = placeDto.Name;
                place.BaseAddress = placeDto.Address.BaseAddress;
                place.DetailAddress = placeDto.Address.DetailAddress;
                place.Categories = placeDto.Categories;
                place.ContactNumber = placeDto.ContactNumber;
                place.OpeningTimes = placeDto.OpeningTimes.Select(x => OpeningTimeRecord.Create(x));
            }
        }

        private async Task ShowRestroomModal(PlaceRecord place)
        {
            var parameters = new DialogParameters
                {
                    { nameof(RestroomModalForm.FormMode), FormMode.Update },
                    { nameof(RestroomModalForm.PlaceId), place.Id }
                };

            var options = new DialogOptions { MaxWidth = MaxWidth.Large };
            var dialog = _dialogService.Show<RestroomModalForm>(string.Empty, parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                var placeId = (long)dialogResult.Data;
                var placeResult = await _placeApiClient.GetPlaceById(placeId);
                if (!_validationService.Validate(placeResult))
                    return;

                var placeDto = placeResult.Data!;
                place.Type = placeDto.Type;
                place.Name = placeDto.Name;
                place.BaseAddress = placeDto.Address.BaseAddress;
                place.DetailAddress = placeDto.Address.DetailAddress;
                place.Categories = placeDto.Categories;
                place.ContactNumber = placeDto.ContactNumber;
                place.OpeningTimes = placeDto.OpeningTimes.Select(x => OpeningTimeRecord.Create(x));
            }
        }

        public async Task OnManagePlaceClick(PlaceRecord place)
        {
            var uri = PageRoutes.Admin.PlaceView.AddRouteParam("PlaceId", place.Id);
            _navManager.NavigateTo(uri);
            await Task.CompletedTask;
        }

        public async Task OnManageProductClick(PlaceRecord place)
        {
            _navManager.NavigateTo(PageRoutes.Admin.PlaceProductList.AddRouteParam("PlaceId", place.Id));
            await Task.CompletedTask;
        }

        public async Task OnClosePlaceClick(PlaceRecord place)
        {
            var parameters = new DialogParameters
            {
                { nameof(ConfirmationDialog.Message), $"'{place.Name}' 을(를) 폐업하시겠습니까?"},
            };

            var options = new DialogOptions { MaxWidth = MaxWidth.Small };
            var dialog = _dialogService.Show<ConfirmationDialog>(string.Empty, parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                var apiResult = await _placeApiClient.ClosePlace(place.Id);
                if (_validationService.Validate(apiResult))
                {
                    await LoadPlaces();
                }
            }
        }

        public async Task OnPageNumberChanged(int pageNumber)
        {
            PageNumber = pageNumber;
            await LoadPlaces();
        }

        public async Task OnRowsPerPageChanged(int rowsPerPage)
        {
            RowsPerPage = rowsPerPage;
            await LoadPlaces();
        }
    }
}
