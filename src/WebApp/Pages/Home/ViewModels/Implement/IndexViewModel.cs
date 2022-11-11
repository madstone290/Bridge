using Bridge.Application.Places.Queries;
using Bridge.WebApp.Api.ApiClients;
using Bridge.WebApp.Pages.Home.Models;
using Bridge.WebApp.Pages.Home.Views.Components;
using Bridge.WebApp.Services;
using Bridge.WebApp.Services.DynamicMap;
using Bridge.WebApp.Services.DynamicMap.Naver;
using Bridge.WebApp.Services.GeoLocation;
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

        private readonly PlaceApiClient _placeApiClient;
        private readonly ISnackbar _snackbar;
        private readonly IDynamicMapService _mapService;
        private readonly IHtmlGeoService _geoService;
        private readonly IReverseGeocodeService _reverseGeocodeService;
        private readonly ICommonJsService _commonJsService;


        public IndexViewModel(PlaceApiClient placeApiClient, ISnackbar snackbar, IDynamicMapService mapService, IHtmlGeoService geoService, IReverseGeocodeService reverseGeocodeService, ICommonJsService commonJsService)
        {
            _placeApiClient = placeApiClient;
            _snackbar = snackbar;
            _mapService = mapService;
            _geoService = geoService;
            _reverseGeocodeService = reverseGeocodeService;
            _commonJsService = commonJsService;
        }

        public string MapElementId { get; } = "MapId";
        public string ProductListElementId { get; } = "ProductList";
        public string PlaceListElementId { get; } = "PlaceList";
        public bool PlaceSearched { get; private set; }
        public bool ProductSearched { get; set; }
        public string SearchText { get; set; } = string.Empty;
        public LatLon? CurrentLocation { get; set; }
        public string? CurrentAddress { get; set; }
        public Place? SelectedPlace { get; set; }
        public Product? SelectedProduct { get; set; }
        public IEnumerable<Place> Places => _places;
        public EventCallback SearchCompleted { get; set; }
        public IHandleEvent Receiver { get; set; } = null!;
        public IEnumerable<Product> Products => _products;
        public ResultTab SelectedTab { get; set; } = ResultTab.Place;
        

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
            _mapService.MyLocationChangedCallback = (new(Receiver, OnMyLocationChanged));
            await _mapService.InitAsync(mapOptions);
        }

        private async void OnSelectedMarkerChangedCallback(string markerId)
        {
            if (Guid.TryParse(markerId, out Guid id))
            {
                var place = Places.FirstOrDefault(x => x.Id == id);
                SelectedPlace = place;

                await _commonJsService.ScrollAsync(PlaceListElementId, id.ToString());
            }
        }

        private async Task OnMyLocationChanged(MapPoint location)
        {
            CurrentLocation = new LatLon(location.Y, location.X);
            await GetCurrentAddressAsync();
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
            //var query = new SearchPlacesQuery()
            //{
            //    SearchText = SearchText,
            //    Latitude = location.Latitude,
            //    Longitude = location.Longitude
            //};
            //var apiResult = await _placeApiClient.SearchPlaces(query);
            //if (!apiResult.Success)
            //{
            //    _snackbar.Add(apiResult.Error, Severity.Error);
            //    return;
            //}
            //if (apiResult.Data == null)
            //{
            //    _snackbar.Add("데이터가 없습니다", Severity.Error);
            //    return;
            //}

            _products.Clear();
            _products.Add(new Product() { Name = "제품1", Distance = 200 });
            _products.Add(new Product() { Name = "제품2", Distance = 300 });
            //_places.AddRange(apiResult.Data.Select(x =>
            //{
            //    var place = Place.Create(x);
            //    if (x.ImagePath != null)
            //        place.ImageUrl = new Uri(_placeApiClient.HttpClient.BaseAddress!, x.ImagePath).ToString();
            //    return place;
            //})
            //.OrderBy(x => x.Distance));


            ProductSearched = true;
        }

        private async Task CreateProductMarkersAsync()
        {
            var markers = Products
                .Where(x => x.Place != null)
                .Select(x => x.Place!)
                .Select(x => new Marker()
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude
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