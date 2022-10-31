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

            /// <summary>
            /// 마커 사용여부
            /// </summary>
            public bool ShowMarker { get; init; }
        }

        private const string JsFile = "/js/naver_map.js";

        #region 자바스크립트 함수 식별자
        private const string InitId = "init";
        private const string CloseId = "close";
        private const string GetMarkerLocationId = "getMarkerLocation";
        private const string AddMarkersId = "addMarkers";
        private const string ClearMarkersId = "clearMarkers";
        #endregion


        private readonly IJSRuntime _jsRuntime;
        private readonly DotNetObjectReference<NaverMapService> _dotNetRef;
        private readonly Dictionary<string, EventCallback<MapPoint>> _centerChangedCallbacks = new();
        private readonly Dictionary<string, EventCallback<MapPoint>> _onClickCallbacks = new();

        private IJSObjectReference? _module;


        public NaverMapService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            _dotNetRef = DotNetObjectReference.Create(this);
        }

        public void SetCenterChangedCallback(string sessionId, EventCallback<MapPoint> callback)
        {
            _centerChangedCallbacks[sessionId] = callback;
        }

        public void SetOnClickCallback(string sessionId, EventCallback<MapPoint> callback)
        {
            _onClickCallbacks[sessionId] = callback;
        }

        [JSInvokable]
        public void OnCenterChanged(string sessionId, double x, double y)
        {
            if (_centerChangedCallbacks.TryGetValue(sessionId, out var callback))
                callback.InvokeAsync(new MapPoint() { X = x, Y = y });
        }


        [JSInvokable]
        public void OnClick(string sessionId, double x, double y)
        {
            if (_onClickCallbacks.TryGetValue(sessionId, out var callback))
                callback.InvokeAsync(new MapPoint() { X = x, Y = y });
        }


        public async Task InitAsync(string sessionId, IMapOptions mapOptions)
        {
            _module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", JsFile);

            var naverMapOptions = (MapOptions)mapOptions;
            await _module.InvokeVoidAsync(InitId, sessionId, _dotNetRef, naverMapOptions.MapId, naverMapOptions.CenterX, naverMapOptions.CenterY, naverMapOptions.ShowMarker);
        }

        public async Task<MapPoint> GetSelectedLocationAsync(string sessionId)
        {
            if (_module == null)
                return new MapPoint();
            return await _module.InvokeAsync<MapPoint>(GetMarkerLocationId, sessionId);
        }

        public async Task AddMarkers(string sessionId, IEnumerable<Marker> markers)
        {
            if (_module == null)
                return;
            await _module.InvokeVoidAsync(AddMarkersId, sessionId, markers);
        }

        public async Task ClearMarkers(string sessionId)
        {
            if (_module == null)
                return;
            await _module.InvokeVoidAsync(ClearMarkersId, sessionId);
        }

        public async Task CloseAsync(string sessionId)
        {
            _centerChangedCallbacks.Remove(sessionId);
            _onClickCallbacks.Remove(sessionId);

            if (_module == null)
                return;
            await _module.InvokeVoidAsync(CloseId, sessionId);
        }

        public async ValueTask DisposeAsync()
        {
            _dotNetRef.Dispose();

            if (_module == null)
                return;
            await _module.DisposeAsync();
        }

     
    }
}