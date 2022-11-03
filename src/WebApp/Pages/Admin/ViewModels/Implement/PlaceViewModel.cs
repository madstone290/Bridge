using Bridge.WebApp.Api.ApiClients.Admin;
using Bridge.WebApp.Pages.Admin.Models;
using Bridge.WebApp.Pages.Admin.Views.Components;
using Bridge.WebApp.Services;
using Bridge.WebApp.Shared;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Linq.Expressions;

namespace Bridge.WebApp.Pages.Admin.ViewModels.Implement
{
    public class PlaceViewModel : IPlaceViewModel
    {
        private readonly PlaceModel _place = new();
        private readonly PlaceModel.Validator _validator = new();
        private readonly PlaceModel _placeBackup = new();
        private readonly List<ProductModel> _products = new();


        private readonly AdminPlaceApiClient _placeApiClient;
        private readonly AdminProductApiClient _productApiClient;
        private readonly IApiResultValidationService _validationService;
        private readonly IDialogService _dialogService;
        private readonly ISnackbar _snackbar;

        public PlaceViewModel(AdminPlaceApiClient placeApiClient, AdminProductApiClient productApiClient, IApiResultValidationService validationService, IDialogService dialogService, ISnackbar snackbar)
        {
            _placeApiClient = placeApiClient;
            _productApiClient = productApiClient;
            _validationService = validationService;
            _dialogService = dialogService;
            _snackbar = snackbar;
        }

        public long PlaceId { get; set; }

        public bool IsBaseInfoValid { get; set; }

        public bool IsOpeningTimeValid { get; set; }

        public bool BaseInfoReadOnly { get; private set; } = true;

        public PlaceModel Place => _place;

        public bool OpeningTimeReadOnly { get; private set; } = true;

        public string SearchText { get; set; } = string.Empty;
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; } = 1;
        public int RowsPerPage { get; set; } = 10;

        public IEnumerable<ProductModel> Products => _products;

        private void CopyPlace(PlaceModel source, PlaceModel target)
        {
            target.Id = source.Id;
            target.Type = source.Type;
            target.Name = source.Name;
            target.BaseAddress = source.BaseAddress;
            target.DetailAddress = source.DetailAddress;
            target.ImageChanged = source.ImageChanged;
            target.ImageName = source.ImageName;
            target.ImageData = source.ImageData;
            target.ImageUrl = source.ImageUrl;
            target.Categories = source.Categories;
            target.ContactNumber = source.ContactNumber;
            target.OpeningTimes = source.OpeningTimes;
        }

        private async Task LoadPlaceAsync()
        {
            var placeTask = _placeApiClient.GetPlaceById(PlaceId);

            await Task.WhenAll(placeTask);

            var placeResult = placeTask.Result;

            if (!_validationService.Validate(placeResult))
                return;

            var placeDto = placeResult.Data!;

            _place.Id = placeDto.Id;
            _place.Type = placeDto.Type;
            _place.Name = placeDto.Name;
            _place.BaseAddress = placeDto.Address.BaseAddress;
            _place.DetailAddress = placeDto.Address.DetailAddress;
            _place.Categories = placeDto.Categories;
            _place.ContactNumber = placeDto.ContactNumber;
            _place.OpeningTimes = placeDto.OpeningTimes.Select(x => OpeningTimeModel.Create(x)).ToList();

            if (placeDto.ImagePath != null)
                _place.ImageUrl = new Uri(_placeApiClient.HttpClient.BaseAddress!, placeDto.ImagePath).ToString();

            CopyPlace(_place, _placeBackup);
        }

        private async Task LoadProductsAsync()
        {
            var productResult = await _productApiClient.GetPaginatedProductList(PlaceId, PageNumber, RowsPerPage);
            if (!_validationService.Validate(productResult))
                return;

            var productsDto = productResult.Data!;

            TotalCount = productsDto.TotalCount;
            PageCount = productsDto.TotalPages;

            _products.Clear();
            _products.AddRange(productsDto.List.Select(x => ProductModel.Create(x)));
        }

        public async Task Initialize()
        {
            var placeTask = LoadPlaceAsync();
            var productTask = LoadProductsAsync();

            await Task.WhenAll(placeTask, productTask);
        }

        public Func<TProperty, Task<IEnumerable<string>>> GetValidation<TProperty>(Expression<Func<PlaceModel, TProperty>> expression)
        {
            return _validator.PropertyValidation(expression);
        }

        public async Task OnUploadFileChange(InputFileChangeEventArgs args)
        {
            var file = args.File;
            var sizeLimit = 50000;
            if (sizeLimit < file.Size)
            {
                _snackbar.Add("50Kb가 넘는 이미지는 사용할 수 없습니다");
                return;
            }

            var format = file.ContentType;
            var buffer = new byte[file.Size];
            using var stream = file.OpenReadStream(file.Size);
            await stream.ReadAsync(buffer);

            var base64 = Convert.ToBase64String(buffer);
            _place.ImageUrl = $"data:{format};base64,{base64}";
            _place.ImageData = buffer;
            _place.ImageName = file.Name;
            _place.ImageChanged = true;
        }

        public async Task OnEditBaseInfoClick()
        {
            BaseInfoReadOnly = false;
            await Task.CompletedTask;
        }

        public async Task OnCancelBaseInfoClick()
        {
            CopyPlace(_placeBackup, _place);
            BaseInfoReadOnly = true;
            await Task.CompletedTask;
        }

        public async Task OnSaveBaseInfoClick()
        {
            if (!IsBaseInfoValid)
                return;

            var result = await _placeApiClient.UpdatePlaceBaseInfo(new Application.Places.Commands.UpdatePlaceBaseInfoCommand()
            {
                Id = _place.Id,
                Address = new Application.Places.Dtos.AddressDto()
                {
                    BaseAddress = _place.BaseAddress,
                    DetailAddress = _place.DetailAddress
                },
                Name = _place.Name,
                Categories = _place.Categories,
                ContactNumber = _place.ContactNumber,
                ImageChanged = _place.ImageChanged,
                ImageName = _place.ImageName,
                ImageData = _place.ImageData,
            });

            if (!_validationService.Validate(result))
            {
                CopyPlace(_placeBackup, _place);
                return;
            }

            CopyPlace(_place, _placeBackup);
            BaseInfoReadOnly = true;
        }

        public Task OnEditOpeningTimeClick()
        {
            OpeningTimeReadOnly = false;
            return Task.CompletedTask;
        }

        public Task OnCancelOpeningTimeClick()
        {
            CopyPlace(_placeBackup, _place);
            OpeningTimeReadOnly = true;
            return Task.CompletedTask;
        }

        public async Task OnSaveOpeningTimeClick()
        {
            if (!IsOpeningTimeValid)
                return;

            var result = await _placeApiClient.UpdatePlaceOpeningTimes(new Application.Places.Commands.UpdatePlaceOpeningTimesCommand()
            {
                Id = _place.Id,
                OpeningTimes = _place.OpeningTimes.Select(t => new Application.Places.Dtos.OpeningTimeDto()
                {
                    Day = t.Day,
                    Dayoff = t.IsDayoff,
                    TwentyFourHours = t.Is24Hours,
                    BreakEndTime = t.BreakEndTime,
                    BreakStartTime = t.BreakStartTime,
                    OpenTime = t.OpenTime,
                    CloseTime = t.CloseTime,
                }).ToList(),
            });

            if (!_validationService.Validate(result))
            {
                CopyPlace(_placeBackup, _place);
                return;
            }

            CopyPlace(_place, _placeBackup);
            OpeningTimeReadOnly = true;
        }

        public async Task OnCreateProductClick()
        {
            var parameters = new DialogParameters();
            parameters.Add(nameof(ProductFormView.PlaceId), _place.Id);

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<ProductFormView>(string.Empty, parameters, options);
            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                await LoadProductsAsync();
            }
        }

        public async Task OnUpdateProductClick(ProductModel product)
        {
            var parameters = new DialogParameters();
            parameters.Add(nameof(ProductFormView.FormMode), FormMode.Update);
            parameters.Add(nameof(ProductFormView.ProductId), product.Id);

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<ProductFormView>(string.Empty, parameters, options);
            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                var productResult = await _productApiClient.GetProductById(product.Id);
                if (!_validationService.Validate(productResult))
                    return;

                var productDto = productResult.Data!;
                var oldProduct = _products.First(x => x.Id == product.Id);
                oldProduct.Name = productDto.Name;
                oldProduct.Type = productDto.Type;
                oldProduct.Price = productDto.Price;
                oldProduct.Categories = productDto.Categories;
            }
        }

        public async Task OnDiscardProductClick(ProductModel product)
        {
            var parameters = new DialogParameters
            {
                { nameof(ConfirmationDialog.Message), $"'{product.Name}' 을(를) 폐기하시겠습니까?"},
            };

            var options = new DialogOptions { MaxWidth = MaxWidth.Small };
            var dialog = _dialogService.Show<ConfirmationDialog>(string.Empty, parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                var apiResult = await _productApiClient.DiscardProduct(product.Id);
                if (_validationService.Validate(apiResult))
                {
                    await LoadProductsAsync();
                }
            }
        }

        public async Task OnPageNumberChanged(int pageNumber)
        {
            PageNumber = pageNumber;
            await LoadProductsAsync();
        }

        public async Task OnRowsPerPageChanged(int rowsPerPage)
        {
            RowsPerPage = rowsPerPage;
            await LoadProductsAsync();
        }
    }
}