using Bridge.Domain.Places.Enums;
using Bridge.Shared.Extensions;
using Bridge.WebApp.Api.ApiClients.Admin;
using Bridge.WebApp.Pages.Admin.Models;
using Bridge.WebApp.Pages.Admin.Views.Components;
using Bridge.WebApp.Services;
using Bridge.WebApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Bridge.WebApp.Pages.Admin.ViewModels.Implement
{
    public class PlaceListViewModel : IPlaceListViewModel
    {
        private const string SelectAll = "전체";
        private readonly Dictionary<PlaceType, string> _placeTypeTexts = new()
        {
            { Domain.Places.Enums.PlaceType.Other, "기타" },
            { Domain.Places.Enums.PlaceType.Pharmacy, "약국" },
            { Domain.Places.Enums.PlaceType.Hospital, "병원" },
            { Domain.Places.Enums.PlaceType.Cafeteria, "카페" },
            { Domain.Places.Enums.PlaceType.Restaurant, "식당" },
            { Domain.Places.Enums.PlaceType.Restroom, "공중화장실" },
        };
        private readonly List<Place> _placeList = new();
        private readonly ExcelOptions _excelOptions = new()
        {
            Columns = typeof(DaeguExcelRestroom).GetProperties()
               .Select(x => new ExcelOptions.Column(x.Name, x.GetCustomAttribute<DisplayAttribute>()?.Name ?? x.Name))
        };

        private readonly AdminRestroomApiClient _restroomApiClient;
        private readonly AdminPlaceApiClient _placeApiClient;
        private readonly IExcelService _excelService;
        private readonly IResultValidationService _validationService;
        private readonly IDialogService _dialogService;
        private readonly NavigationManager _navManager;

        public PlaceListViewModel(AdminRestroomApiClient restroomApiClient, AdminPlaceApiClient placeApiClient, IExcelService excelService, IResultValidationService validationService, IDialogService dialogService, NavigationManager navManager)
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
        public IEnumerable<Place> Places => _placeList;

        private async Task LoadPlaces()
        {
            var result = await _placeApiClient.GetPaginatedPlaceList(SearchText, PlaceType, PageNumber, RowsPerPage);
            if (!_validationService.Validate(result))
                return;

            var placeList = result.Data!;

            TotalCount = placeList.TotalCount;
            PageNumber = placeList.PageNumber;
            PageCount = placeList.TotalPages;

            _placeList.Clear();
            _placeList.AddRange(placeList.List.Select(x => Place.CreateFromReadModel(x)));
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
                { nameof(PlaceFormView.FormMode), FormMode.Create }
            };

            var options = new DialogOptions { MaxWidth = MaxWidth.Large };
            var dialog = _dialogService.Show<PlaceFormView>(string.Empty, parameters, options);
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
                { nameof(RestroomFormView.FormMode), FormMode.Create }
            };

            var options = new DialogOptions { MaxWidth = MaxWidth.Large };
            var dialog = _dialogService.Show<RestroomFormView>(string.Empty, parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                await LoadPlaces();
            }
        }

        public async Task OnRestroomExcelDownloadClick()
        {
            await _excelService.DownloadAsync("화장실 폼.xlsx", new List<DaeguExcelRestroom>(), _excelOptions);
        }

        public async Task OnRestroomExcelUploadClick()
        {
            // 엑셀에서 데이터 읽기
            var restrooms = await _excelService.UploadAsync<DaeguExcelRestroom>(_excelOptions);
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
                        new Application.Places.Dtos.OpeningTimeDto(){ Day = DayOfWeek.Monday, Is24Hours = x.Is24Hours, OpenTime = x.OpenTime, CloseTime = x.CloseTime },
                        new Application.Places.Dtos.OpeningTimeDto(){ Day = DayOfWeek.Tuesday, Is24Hours = x.Is24Hours, OpenTime = x.OpenTime, CloseTime = x.CloseTime },
                        new Application.Places.Dtos.OpeningTimeDto(){ Day = DayOfWeek.Wednesday, Is24Hours = x.Is24Hours, OpenTime = x.OpenTime, CloseTime = x.CloseTime },
                        new Application.Places.Dtos.OpeningTimeDto(){ Day = DayOfWeek.Thursday, Is24Hours = x.Is24Hours, OpenTime = x.OpenTime, CloseTime = x.CloseTime },
                        new Application.Places.Dtos.OpeningTimeDto(){ Day = DayOfWeek.Friday, Is24Hours = x.Is24Hours, OpenTime = x.OpenTime, CloseTime = x.CloseTime },
                        new Application.Places.Dtos.OpeningTimeDto(){ Day = DayOfWeek.Saturday, Is24Hours = x.Is24Hours, OpenTime = x.OpenTime, CloseTime = x.CloseTime },
                        new Application.Places.Dtos.OpeningTimeDto(){ Day = DayOfWeek.Sunday, Is24Hours = x.Is24Hours, OpenTime = x.OpenTime, CloseTime = x.CloseTime },
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

        public async Task OnShowOpeningTimeClick(Place place)
        {
            place.ShowOpeningTimes = !place.ShowOpeningTimes;
            await Task.CompletedTask;
        }

        public async Task OnEditPlaceClick(Place place)
        {
            if (place.Type == Domain.Places.Enums.PlaceType.Restroom)
            {
                await ShowRestroomModal(place);
                return;
            }

            var parameters = new DialogParameters
            {
                { nameof(PlaceFormView.FormMode), FormMode.Update },
                { nameof(PlaceFormView.PlaceId), place.Id }
            };

            var options = new DialogOptions { MaxWidth = MaxWidth.Large };
            var dialog = _dialogService.Show<PlaceFormView>(string.Empty, parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                var placeId = (Guid)dialogResult.Data;
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
                place.OpeningTimes = placeDto.OpeningTimes.Select(x => OpeningTime.Create(x));
            }
        }

        private async Task ShowRestroomModal(Place place)
        {
            var parameters = new DialogParameters
                {
                    { nameof(RestroomFormView.FormMode), FormMode.Update },
                    { nameof(RestroomFormView.PlaceId), place.Id }
                };

            var options = new DialogOptions { MaxWidth = MaxWidth.Large };
            var dialog = _dialogService.Show<RestroomFormView>(string.Empty, parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                var placeId = (Guid)dialogResult.Data;
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
                place.OpeningTimes = placeDto.OpeningTimes.Select(x => OpeningTime.Create(x));
            }
        }

        public async Task OnManagePlaceClick(Place place)
        {
            var uri = PageRoutes.Admin.PlaceView.AddRouteParam("PlaceId", place.Id);
            _navManager.NavigateTo(uri);
            await Task.CompletedTask;
        }

        public async Task OnManageProductClick(Place place)
        {
            _navManager.NavigateTo(PageRoutes.Admin.PlaceProductList.AddRouteParam("PlaceId", place.Id));
            await Task.CompletedTask;
        }

        public async Task OnClosePlaceClick(Place place)
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
