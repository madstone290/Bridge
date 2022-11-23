using Bridge.Application.Places.Queries;
using Bridge.Application.Products.Queries;
using Bridge.WebApp.Api.ApiClients;
using Bridge.WebApp.Pages.Admin.Views;
using Bridge.WebApp.Pages.Home.Models;
using Bridge.WebApp.Pages.Home.Views;
using Bridge.WebApp.Services;
using Bridge.WebApp.Services.DynamicMap;
using Bridge.WebApp.Services.DynamicMap.Naver;
using Bridge.WebApp.Services.GeoLocation;
using Bridge.WebApp.Services.Identity;
using Bridge.WebApp.Services.ReverseGeocode;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;

namespace Bridge.WebApp.Pages.Home.ViewModels.Implement
{
    public class IndexViewModel : IIndexViewModel
    {
        private readonly List<Place> _places = new();
        private readonly List<Product> _products = new();

        private readonly ProductApiClient _productApiClient;
        private readonly PlaceApiClient _placeApiClient;
        private readonly ISnackbar _snackbar;
        private readonly IDynamicMapService _mapService;
        private readonly IHtmlGeoService _geoService;
        private readonly IReverseGeocodeService _reverseGeocodeService;
        private readonly ICommonJsService _commonJsService;
        private readonly IDialogService _dialogService;
        private readonly IAuthService _authService;
        private readonly NavigationManager _navigationManager;

        private Place? _selectedPlace;

        public IndexViewModel(ProductApiClient productApiClient,
                              PlaceApiClient placeApiClient,
                              ISnackbar snackbar,
                              IDynamicMapService mapService,
                              IHtmlGeoService geoService,
                              IReverseGeocodeService reverseGeocodeService,
                              ICommonJsService commonJsService,
                              IDialogService dialogService,
                              IPlaceDetailViewModel placeDetailVM,
                              IAuthService authService,
                              NavigationManager navigationManager)
        {
            _productApiClient = productApiClient;
            _placeApiClient = placeApiClient;
            _snackbar = snackbar;
            _mapService = mapService;
            _geoService = geoService;
            _reverseGeocodeService = reverseGeocodeService;
            _commonJsService = commonJsService;
            _dialogService = dialogService;
            PlaceDetailVM = placeDetailVM;
            _authService = authService;
            _navigationManager = navigationManager;
        }

        public IPlaceDetailViewModel PlaceDetailVM { get; set; } = null!;
        public string MapElementId { get; } = "MapId";
        public string ProductListElementId { get; } = "ProductList";
        public string PlaceListElementId { get; } = "PlaceList";
        public bool PlaceSearched { get; private set; }
        public bool ProductSearched { get; set; }
        public string SearchText { get; set; } = string.Empty;
        public LatLon? CurrentLocation { get; set; }
        public string? CurrentAddress { get; set; }

        public Place? SelectedPlace
        {
            get => _selectedPlace;
            set
            {
                _selectedPlace = value;
                if (value != null)
                {
                    PlaceDetailVM.Place = value;
                    PlaceDetailVM.LoadProducts();
                }
            }
        }

        public Product? SelectedProduct { get; set; }
        public IEnumerable<Place> Places => _places;
        public EventCallback SearchCompleted { get; set; }
        public IHandleEvent Receiver { get; set; } = null!;
        public IEnumerable<Product> Products => _products;
        public ResultTab SelectedTab { get; set; }
        

        public async Task InitAsync()
        {
            await GetCurrentLocationAsync();
            await Task.WhenAll(GetCurrentAddressAsync(), InitDynamicMapAsync());

            if (CurrentLocation != null)
                await _mapService.SetMyLocationAsync(new MapPoint() { X = CurrentLocation.Longitude, Y = CurrentLocation.Latitude });
        }

        private async Task GetCurrentLocationAsync()
        {
            var geoResult = await _geoService.GetLocationAsync();
            if (geoResult.Success)
            {
                var point = geoResult.Data!;
                CurrentLocation = new LatLon(point.Latitude, point.Longitude);
            }
            else
            {
                _snackbar.Add(geoResult.Error, Severity.Error);
            }
        }

        private async Task GetCurrentAddressAsync()
        {
            if (CurrentLocation == null)
            {
                CurrentAddress = "위치확인 불가";
                return;
            }

            var addressResult = await _reverseGeocodeService.GetAddressAsync(CurrentLocation.Latitude, CurrentLocation.Longitude);
            CurrentAddress = addressResult.Data;
        }

        private async Task InitDynamicMapAsync()
        {
            var mapOptions = new NaverMapService.MapOptions()
            {
                MapId = MapElementId,
                CenterX = CurrentLocation?.Longitude,
                CenterY = CurrentLocation?.Latitude
            };

            _mapService.SetOnSelectedMarkerChangedCallback(new(Receiver, OnSelectedMarkerChangedCallback));
            _mapService.ChangeMyLocationClickCallback = (new(Receiver, OnChangeMyLocationClick));
            _mapService.AddPlaceClickCallback = new(Receiver, OnAddPlaceClick);
            await _mapService.InitAsync(mapOptions);
        }

        private async void OnSelectedMarkerChangedCallback(string markerId)
        {
            if (Guid.TryParse(markerId, out Guid placeId))
            {
                if(SelectedTab == ResultTab.Product)
                {
                    var product = Products.First(x => x.PlaceId == placeId);
                    SelectedProduct = product;
                    await _commonJsService.ScrollAsync(ProductListElementId, product.Id.ToString());
                }
                else
                {
                    var place = Places.First(x => x.Id == placeId);
                    SelectedPlace = place;
                    await _commonJsService.ScrollAsync(PlaceListElementId, place.Id.ToString());
                }
            }
        }

        private async Task OnChangeMyLocationClick(MapPoint location)
        {
            CurrentLocation = new LatLon(location.Y, location.X);
            await GetCurrentAddressAsync();
        }

        public async Task OnAddPlaceClick()
        {
            var state = await _authService.GetAuthStateAsync();
            if (!state.IsAuthenticated)
            {
                // redirect to login page
                _navigationManager.NavigateTo(PageRoutes.Identity.Login);
                return;
            }

            var parameters = new DialogParameters 
            {
            };
            var options = new DialogOptions { MaxWidth = MaxWidth.Large };
            var dialog = _dialogService.Show<PlaceAddView>(string.Empty, parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                await OnSearchClick();
            }
        }

        private bool CanSearch()
        {
            if(string.IsNullOrEmpty(SearchText))
                return false;
            if (CurrentLocation == null)
                return false;
            return true;
        }

        public async Task OnSearchClick()
        {
            if (!CanSearch())
                return;

            await ClearResultAsync();

            await SearchAsync(CurrentLocation!);

            if (SearchCompleted.HasDelegate)
                await SearchCompleted.InvokeAsync();
        }

        /// <summary>
        /// 검색 실행
        /// </summary>
        /// <returns></returns>
        private async Task SearchAsync(LatLon location)
        {
            if (SelectedTab == ResultTab.Product)
            {
                await SearchProductsAsync(location);
                await CreateProductMarkersAsync();
            }
            else
            {
                await SearchPlacesAsync(location);
                await CreatePlaceMarkers();
            }
        }

        private async Task SearchProductsAsync(LatLon location)
        {
            var query = new SearchProductsQuery()
            {
                SearchText = SearchText,
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };
            var apiResult = await _productApiClient.SearchAsync(query);
            if (!apiResult.Success)
            {
                _snackbar.Add(apiResult.Error, Severity.Error);
                return;
            }
            if (apiResult.Data == null)
            {
                _snackbar.Add("데이터가 없습니다", Severity.Error);
                return;
            }

            _products.Clear();
            _products.AddRange(apiResult.Data.Select(x => Product.Create(x))
                .OrderBy(x => x.Place?.Distance));


            ProductSearched = true;
        }

        private async Task CreateProductMarkersAsync()
        {
            var markers = Products
                .Where(product => product.Place != null)
                .DistinctBy(x=> x.PlaceId)
                .Select(product => product.Place!)
                .Select(place => new Marker()
                {
                    Id = place.Id.ToString(),
                    Name = place.Name,
                    Latitude = place.Latitude,
                    Longitude = place.Longitude
                });

            await _mapService.ClearPlaceMarkersAsync();
            await _mapService.AddPlaceMarkersAsync(markers);
        }

        private async Task SearchPlacesAsync(LatLon location)
        {
            var query = new SearchPlacesQuery()
            {
                SearchText = SearchText,
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };
            var apiResult = await _placeApiClient.SearchPlaces(query);
            if (!apiResult.Success)
            {
                _snackbar.Add(apiResult.Error, Severity.Error);
                return;
            }
            if (apiResult.Data == null)
            {
                _snackbar.Add("데이터가 없습니다", Severity.Error);
                return;
            }

            _places.Clear();
            _places.AddRange(apiResult.Data.Select(x =>
            {
                var place = Place.Create(x);
                if (x.ImagePath != null)
                    place.ImageUrl = new Uri(_placeApiClient.HttpClient.BaseAddress!, x.ImagePath).ToString();
                return place;
            })
            .OrderBy(x => x.Distance));

            PlaceSearched = true;
        }

        private async Task CreatePlaceMarkers()
        {
            var markers = Places.Select(x => new Marker()
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Latitude = x.Latitude,
                Longitude = x.Longitude
            });
            await _mapService.ClearPlaceMarkersAsync();
            await _mapService.AddPlaceMarkersAsync(markers);
        }

        public async Task OnSelectedTabChanged(ResultTab tab)
        {
            SelectedTab = tab;
            if (!CanSearch())
                return;

            if (tab == ResultTab.Product)
            {
                if(!ProductSearched)
                    await SearchProductsAsync(CurrentLocation!);
                await CreateProductMarkersAsync();
            }
            else if(tab == ResultTab.Place)
            {
                if(!PlaceSearched)
                    await SearchPlacesAsync(CurrentLocation!);
                await CreatePlaceMarkers();
            }
            
        }

        public async Task OnClearClick()
        {
            SearchText = string.Empty;

            await ClearResultAsync();
        }

        private async Task ClearResultAsync()
        {
            _places.Clear();
            _products.Clear();
            PlaceSearched = false;
            ProductSearched = false;
            SelectedPlace = null;

            await CreatePlaceMarkers();
        }

        public async Task OnSearchFieldKeyUp(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                await OnSearchClick();
            }
        }

        public async Task OnProductSelected(Product product)
        {
            SelectedProduct = product;
            if (product.Place == null)
                return;
            var place = product.Place;
            await _mapService.SelectPlaceMarkerAsync(place.Id.ToString());
            await _mapService.MoveAsync(place.Latitude, place.Longitude);
        }

        public async Task OnPlaceSelected(Place place)
        {
            SelectedPlace = place;
            await _mapService.SelectPlaceMarkerAsync(place.Id.ToString());
            await _mapService.MoveAsync(place.Latitude, place.Longitude);
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            try
            {
                await _mapService.DisposeMapAsync();
            }
            // 무시. F5 갱신시에는 SignalR커넥션이 끊어지므로 발생하는 오류
            catch (JSDisconnectedException) { }

            GC.SuppressFinalize(this);
        }

    }
}