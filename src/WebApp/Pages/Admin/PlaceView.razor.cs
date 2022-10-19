using Bridge.WebApp.Api.ApiClients.Admin;
using Bridge.WebApp.Pages.Admin.Components;
using Bridge.WebApp.Pages.Admin.Models;
using Bridge.WebApp.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bridge.WebApp.Pages.Admin
{
    public partial class PlaceView
    {
        private MudForm? _baseInfoForm;

        /// <summary>
        /// 기본정보 읽기전용 상태
        /// </summary>
        private bool _baseInfoReadOnly = true;

        private MudForm? _openingTimeForm;

        /// <summary>
        /// 영업시간 읽기전용 상태
        /// </summary>
        private bool _openingTimeReadOnly = true;


        private readonly PlaceFormModel _place = new();
        private readonly PlaceFormModel _placeBackup = new();
        private readonly PlaceFormModel.Validator _validator = new();
        private readonly List<ProductModel> _products = new();

        /// <summary>
        /// 제품 검색어
        /// </summary>
        private string _searchString = string.Empty;

        private int _totalCount;
        private int _pageCount;
        private int _pageNumber = 1;
        private int _rowsPerPage = 10;

        /// <summary>
        /// 장소 아이디
        /// </summary>
        [Parameter]
        public long PlaceId { get; set; }

        [Inject]
        public AdminPlaceApiClient PlaceApiClient { get; set; } = null!;

        [Inject]
        public AdminProductApiClient ProductApiClient { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            var placeTask = LoadPlaceAsync();
            var productTask = LoadProductsAsync();

            await Task.WhenAll(placeTask, productTask);
        }

        private void EditBaseInfo_Click()
        {
            _baseInfoReadOnly = false;
        }

        private async void SaveBaseInfo_Click()
        {
            await _baseInfoForm!.Validate();
            if (!_baseInfoForm.IsValid)
                return;

            var result = await PlaceApiClient.UpdatePlaceBaseInfo(new Application.Places.Commands.UpdatePlaceBaseInfoCommand()
            {
                Id = _place.Id,
                Address = new Application.Places.Dtos.AddressDto()
                {
                    BaseAddress = _place.BaseAddress,
                    DetailAddress = _place.DetailAddress
                },
                Name = _place.Name,
                Categories = _place.Categories,
                ContactNumber  = _place.ContactNumber,
            });

            if (!ValidationService.Validate(result))
            {
                PlaceFormModel.Copy(_placeBackup, _place);
                return;
            }

            PlaceFormModel.Copy(_place, _placeBackup);
            _baseInfoReadOnly = true;
            StateHasChanged();
        }

        private void CancelBaseInfo_Click()
        {
            PlaceFormModel.Copy(_placeBackup, _place);
            _baseInfoReadOnly = true;
            StateHasChanged();
        }

        private void EditOpeningTime_Click()
        {
            _openingTimeReadOnly = false;
            StateHasChanged();
        }

        private async void SaveOpeningTime_Click()
        {
            await _openingTimeForm!.Validate();
            if (!_openingTimeForm.IsValid)
                return;

            var result = await PlaceApiClient.UpdatePlaceOpeningTimes(new Application.Places.Commands.UpdatePlaceOpeningTimesCommand()
            {
                Id = _place.Id,
                OpeningTimes = _place.OpeningTimes.Select(t => new Application.Places.Dtos.OpeningTimeDto()
                {
                    Day = t.Day,
                    Dayoff = t.Dayoff,
                    TwentyFourHours = t.TwentyFourHours,
                    BreakEndTime = t.BreakEndTime,
                    BreakStartTime = t.BreakStartTime,
                    OpenTime = t.OpenTime,
                    CloseTime = t.CloseTime,
                }).ToList(),
            });

            if (!ValidationService.Validate(result))
            {
                PlaceFormModel.Copy(_placeBackup, _place);
                return;
            }

            PlaceFormModel.Copy(_place, _placeBackup);
            _openingTimeReadOnly = true;
            StateHasChanged();
        }

        private void CancelOpeningTime_Click()
        {
            PlaceFormModel.Copy(_placeBackup, _place);
            _openingTimeReadOnly = true;
            StateHasChanged();
        }

        private async void ProductCreate_Click()
        {
            var parameters = new DialogParameters();
            parameters.Add(nameof(ProductModalForm.PlaceId), _place.Id);

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = DialogService.Show<ProductModalForm>(string.Empty, parameters, options);
            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                await LoadProductsAsync();
            }
        }

        private async void UpdateProduct_Click(ProductModel product)
        {
            var parameters = new DialogParameters();
            parameters.Add(nameof(ProductModalForm.FormMode), FormMode.Update);
            parameters.Add(nameof(ProductModalForm.ProductId), product.Id);

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = DialogService.Show<ProductModalForm>(string.Empty, parameters, options);
            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                var productResult = await ProductApiClient.GetProductById(product.Id);
                if (!ValidationService.Validate(productResult))
                    return;

                var productDto = productResult.Data!;
                var oldProduct = _products.First(x => x.Id == product.Id);
                oldProduct.Name = productDto.Name;
                oldProduct.Type = productDto.Type;
                oldProduct.Price = productDto.Price;
                oldProduct.Categories = productDto.Categories;
                StateHasChanged();
            }
        }


        private async void PageNumberChanged(int pageNumber)
        {
            _pageNumber = pageNumber;
            await LoadProductsAsync();
        }

        private async void RowsPerPageChanged(int rowsPerPage)
        {
            _rowsPerPage = rowsPerPage;
            await LoadProductsAsync();
        }

        private async Task LoadPlaceAsync()
        {
            var placeTask = PlaceApiClient.GetPlaceById(PlaceId);

            await Task.WhenAll(placeTask);

            var placeResult = placeTask.Result;

            if (!ValidationService.Validate(placeResult))
                return;

            var placeDto = placeResult.Data!;

            _place.Id = placeDto.Id;
            _place.Type = placeDto.Type;
            _place.Name = placeDto.Name;
            _place.BaseAddress = placeDto.Address.BaseAddress;
            _place.DetailAddress = placeDto.Address.DetailAddress;
            _place.Categories = placeDto.Categories;
            _place.ContactNumber = placeDto.ContactNumber;
            _place.OpeningTimes = placeDto.OpeningTimes.Select(x => OpeningTimeFormModel.Create(x));

            PlaceFormModel.Copy(_place, _placeBackup);
        }

        private async Task LoadProductsAsync()
        {
            var productResult = await ProductApiClient.GetPaginatedProductList(PlaceId, _pageNumber, _rowsPerPage);
            if (!ValidationService.Validate(productResult))
                return;

            var productsDto = productResult.Data!;

            _totalCount = productsDto.TotalCount;
            _pageCount = productsDto.TotalPages;

            _products.Clear();
            _products.AddRange(productsDto.List.Select(x => ProductModel.Create(x)));
            StateHasChanged();
        }

        private async void DiscardProduct_Click(ProductModel product)
        {
            var parameters = new DialogParameters
            {
                { nameof(ConfirmationDialog.Message), $"'{product.Name}' 을(를) 폐기하시겠습니까?"},
            };

            var options = new DialogOptions { MaxWidth = MaxWidth.Small };
            var dialog = DialogService.Show<ConfirmationDialog>(string.Empty, parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                var apiResult = await ProductApiClient.DiscardProduct(product.Id);
                if (ValidationService.Validate(apiResult))
                {
                    await LoadProductsAsync();
                }
            }
        }

    }
}
