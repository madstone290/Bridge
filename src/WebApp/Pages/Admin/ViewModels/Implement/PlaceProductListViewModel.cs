using Bridge.Application.Places.ReadModels;
using Bridge.Shared.Extensions;
using Bridge.WebApp.Api.ApiClients.Admin;
using Bridge.WebApp.Pages.Admin.Models;
using Bridge.WebApp.Services;
using Microsoft.AspNetCore.Components;

namespace Bridge.WebApp.Pages.Admin.ViewModels.Implement
{
    public class PlaceProductListViewModel : IPlaceProductListViewModel
    {
        private readonly PlaceReadModel _place = new();
        private readonly List<ProductModel> _products = new();

        private readonly AdminPlaceApiClient _placeApiClient;
        private readonly AdminProductApiClient _productApiClient;
        private readonly NavigationManager _navManager;
        private readonly IApiResultValidationService _validationService;

        public PlaceProductListViewModel(AdminPlaceApiClient placeApiClient, AdminProductApiClient productApiClient, NavigationManager navManager, IApiResultValidationService validationService)
        {
            _placeApiClient = placeApiClient;
            _productApiClient = productApiClient;
            _navManager = navManager;
            _validationService = validationService;
        }

        public long PlaceId { get; set; }
        public PlaceReadModel Place => _place;
        public string SearchText { get; set; } = string.Empty;
        public IEnumerable<ProductModel> Products => _products;

        private async Task LoadData()
        {
            var placeTask = _placeApiClient.GetPlaceById(PlaceId);
            var productTask = _productApiClient.GetProductList(PlaceId);

            await Task.WhenAll(placeTask, productTask);

            var placeResult = placeTask.Result;
            var productResult = productTask.Result;

            if (!_validationService.Validate(placeTask.Result) || !_validationService.Validate(productResult))
                return;

            var placeDto = placeResult.Data!;
            _place.Name = placeDto.Name;
            _place.Address = placeDto.Address;

            var productListDto = productResult.Data!;
            _products.Clear();
            _products.AddRange(productListDto.OrderByDescending(x => x.CreationDateTime).Select(x => ProductModel.Create(x)));
        }

        public async Task Initialize()
        {
            await LoadData();
        }

        public async Task OnCreateClick()
        {
            _navManager.NavigateTo(PageRoutes.Admin.PlaceProductCreate.AddRouteParam("PlaceId", PlaceId));
            await Task.CompletedTask;
        }

        public async Task OnLoadClick()
        {
            await LoadData();
        }

        public bool FilterProduct(ProductModel product)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                return true;
            return product.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true ||
                product.Type.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true ||
                product.Price?.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}
