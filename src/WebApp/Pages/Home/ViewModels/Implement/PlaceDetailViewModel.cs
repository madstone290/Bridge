using Bridge.WebApp.Api.ApiClients;
using Bridge.WebApp.Pages.Common.Models;
using Bridge.WebApp.Pages.Common.Views;
using Bridge.WebApp.Services;
using Bridge.WebApp.Services.Identity;
using MudBlazor;
using System.ComponentModel;

namespace Bridge.WebApp.Pages.Home.ViewModels.Implement
{
    public class PlaceDetailViewModel : IPlaceDetailViewModel
    {
        private readonly ProductApiClient _productApiClient;
        private readonly IResultValidationService _resultValidationService;
        private readonly IDialogService _dialogService;
        private readonly IAuthService _authService;


        private Place _place = new();
        private List<Product> _products = new();
        /// <summary>
        /// 로그인한 사용자의 아이디
        /// </summary>
        private string _userId = string.Empty;

        public PlaceDetailViewModel(ProductApiClient productApiClient, IResultValidationService resultValidationService, IDialogService dialogService, IAuthService authService)
        {
            _productApiClient = productApiClient;
            _resultValidationService = resultValidationService;
            _dialogService = dialogService;
            _authService = authService;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Place Place
        {
            get => _place;
            set
            {
                _place = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Place)));
            }
        }

        public IEnumerable<Product> Products
        {
            get => _products;
            set
            {
                _products = new List<Product>(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Products)));
            }
        }

        public bool IsPlaceOwner => !string.IsNullOrWhiteSpace(_userId) && _userId == Place.OwnerId;

        public async Task LoadProducts()
        {
            var result = await _productApiClient.GetProductList(Place.Id);
            if (_resultValidationService.Validate(result))
            {
                Products = result.Data!.Select(x => Product.Create(x));
            }
        }

        public async Task OnAddProductClick()
        {
            var parameters = new DialogParameters
            {
                { nameof(ProductFormView.FormMode), FormMode.Create },
                { nameof(ProductFormView.PlaceId), Place.Id }
            };

            var options = new DialogOptions { MaxWidth = MaxWidth.Large };
            var dialog = _dialogService.Show<ProductFormView>(string.Empty, parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                await LoadProducts();
            }
        }

        public async Task LoadUserAsync()
        {
            var authState = await _authService.GetAuthStateAsync();
            _userId = authState.UserId;
        }
    }
}