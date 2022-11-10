using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bridge.WebApp.Services.DynamicMap.Naver
{
    public class NaverMapService : IDynamicMapService
    {
        public class MapOptions : IMapOptions
        {
            /// <summary>
            /// 맵을 초기화할 div 요소 아이디
            /// </summary>
            public string MapId { get; init; } = string.Empty;

            /// <summary>
            /// 경도. X축 중심위치
            /// </summary>
            public double? CenterX { get; init; }

            /// <summary>
            /// 위도. Y축 중심위치
            /// </summary>
            public double? CenterY { get; init; }
        }

        private const string JsFile = "/js/naver_map.js";

        #region 자바스크립트 함수 식별자
        private const string InitId = "init";
        private const string AddPlaceMarkersId = "addPlaceMarkers";
        private const string ClearPlaceMarkersId = "clearPlaceMarkers";
        private const string SelectPlaceMarkerId = "selectPlaceMarker";
        private const string MoveId = "move";
        private const string GetMyLocationId = "getMyLocation";
        private const string SetMyLocationId = "setMyLocation";
        private const string DisposeMapId = "disposeMap";
        #endregion


        private readonly IJSRuntime _jsRuntime;
        private readonly DotNetObjectReference<NaverMapService> _dotNetRef;

        private EventCallback<MapPoint> _centerChangedCallback;
        private EventCallback<string> _onSelectedMarkerChangedCallback;
        private EventCallback<Tuple<string, MapPoint>> _onContextMenuClickedCallback;

        private IJSObjectReference? _module;

        public NaverMapService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            _dotNetRef = DotNetObjectReference.Create(this);
        }

        private IJSObjectReference Module
        {
            get
            {
                if (_module == null)
                    throw new Exception("JS모듈이 초기화되지 않았습니다");
                return _module;
            }
        }

        public void SetCenterChangedCallback(EventCallback<MapPoint> callback)
        {
            _centerChangedCallback = callback;
        }

        public void SetOnSelectedMarkerChangedCallback(EventCallback<string> callback)
        {
            _onSelectedMarkerChangedCallback = callback;
        }

        public void SetOnContextMenuClickedCallback(EventCallback<Tuple<string, MapPoint>> callback)
        {
            _onContextMenuClickedCallback = callback;
        }

        [JSInvokable]
        public void OnCenterChanged(double x, double y)
        {
            if(_centerChangedCallback.HasDelegate)
                _centerChangedCallback.InvokeAsync(new MapPoint() { X = x, Y = y });
        }

        [JSInvokable]
        public void OnSelectedPlaceMarkerChanged(string markerId)
        {
            if (_onSelectedMarkerChangedCallback.HasDelegate)
                _onSelectedMarkerChangedCallback.InvokeAsync(markerId);
        }

        [JSInvokable]
        public void OnContextMenuClicked(string menuId, double x, double y)
        {
            if (_onContextMenuClickedCallback.HasDelegate)
                _onContextMenuClickedCallback.InvokeAsync(new Tuple<string, MapPoint>(menuId, new MapPoint() {  X = x, Y = y }));
        }


        public async Task InitAsync(IMapOptions mapOptions)
        {
            _module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", JsFile);

            var naverMapOptions = (MapOptions)mapOptions;
            await _module.InvokeVoidAsync(InitId, _dotNetRef, naverMapOptions.MapId, naverMapOptions.CenterX, naverMapOptions.CenterY);
        }

        public async Task<MapPoint> SetMyLocationAsync(MapPoint location)
        {
            return await Module.InvokeAsync<MapPoint>(SetMyLocationId, location);
        }

        public async Task<MapPoint> GetMyLocationAsync()
        {
            return await Module.InvokeAsync<MapPoint>(GetMyLocationId);
        }

        public async Task AddPlaceMarkersAsync(IEnumerable<Marker> markers)
        {
            await Module.InvokeVoidAsync(AddPlaceMarkersId, markers);
        }

        public async Task ClearPlaceMarkersAsync()
        {
            await Module.InvokeVoidAsync(ClearPlaceMarkersId);
        }

        public async Task SelectPlaceMarkerAsync(string markerId)
        {
            await Module.InvokeVoidAsync(SelectPlaceMarkerId, markerId);
        }

        public async Task MoveAsync(double latitude, double longitude)
        {
            await Module.InvokeVoidAsync(MoveId, latitude, longitude);
        }

        public async Task DisposeMapAsync()
        {
            await Module.InvokeVoidAsync(DisposeMapId);
        }

        public async ValueTask DisposeAsync()
        {
            _dotNetRef.Dispose();

            if (_module != null)
                await _module.DisposeAsync();
        }

   
    }
}