using Bridge.WebApp.Api.ApiClients;
using Bridge.WebApp.Pages.Home.Models;
using Bridge.WebApp.Services;
using System.ComponentModel;

namespace Bridge.WebApp.Pages.Home.ViewModels.Implement
{
    public class PlaceDetailViewModel : IPlaceDetailViewModel
    {

        private readonly ProductApiClient _productApiClient;
        private readonly IResultValidationService _resultValidationService;

        private Place _place = new();
        private List<Product> _products = new();

        public PlaceDetailViewModel(ProductApiClient productApiClient, IResultValidationService resultValidationService)
        {
            _productApiClient = productApiClient;
            _resultValidationService = resultValidationService;
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



        public async Task LoadProducts()
        {
            var result = await _productApiClient.GetProductList(Place.Id);
            if (_resultValidationService.Validate(result))
            {
                Products = result.Data!.Select(x => Product.Create(x));
            }
        }
    }
}