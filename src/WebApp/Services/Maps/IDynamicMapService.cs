using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bridge.WebApp.Services.Maps
{
    /// <summary>
    /// 동적 지도 서비스
    /// </summary>
    public interface IDynamicMapService : IAsyncDisposable
    {
        /// <summary>
        /// 지도 중심위치가 변경될 경우 호출할 콜백을 등록한다.
        /// </summary>
        /// <param name="sessionId">세션 아이디. 서비스에서 맵 식별을 위해 사용한다.</param>
        /// <param name="callback">이벤트 콜백</param>
        void SetCenterChangedCallback(string sessionId, EventCallback<MapPoint> callback);

        /// <summary>
        /// 마우스 클릭 이벤트 콜백을 등록한다.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="callback"></param>
        void SetOnClickCallback(string sessionId, EventCallback<MapPoint> callback);

        /// <summary>
        /// 맵을 초기화한다
        /// </summary>
        /// <param name="sessionId">세션 아이디. 서비스에서 맵 식별을 위해 사용한다.</param>
        /// <param name="mapOptions">맵 옵션</param>
        /// <returns></returns>
        Task InitAsync(string sessionId, IMapOptions mapOptions);

        /// <summary>
        /// 맵을 닫는다
        /// </summary>
        /// <param name="sessionId">세션 아이디. 서비스에서 맵 식별을 위해 사용한다.</param>
        /// <returns></returns>
        Task CloseAsync(string sessionId);

        /// <summary>
        /// 사용자가 선택한 위치를 가져온다
        /// </summary>
        /// <param name="sessionId">세션 아이디. 서비스에서 맵 식별을 위해 사용한다.</param>
        /// <returns>사용자가 선택한 위치</returns>
        Task<MapPoint> GetSelectedLocationAsync(string sessionId);
    }

    /// <summary>
    /// 맵 옵션 마커 인터페이스
    /// </summary>
    public interface IMapOptions
    {

    }

    /// <summary>
    /// 지도상의 위치
    /// </summary>
    public class MapPoint
    {
        public double X { get; init; }
        public double Y { get; init; }
    }

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
