using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bridge.WebApp.Services
{
    /// <summary>
    /// 동적 지도 서비스
    /// </summary>
    public interface IDynamicMapService : IAsyncDisposable
    {
        /// <summary>
        /// 위치 변경 콜백
        /// </summary>
        public EventCallback<MapPoint> LocationChanged { get; set; }

        /// <summary>
        /// 주소 변경 콜백
        /// </summary>
        public EventCallback<string> AddressChanged { get; set; }

        /// <summary>
        /// 맵을 초기화한다
        /// </summary>
        /// <returns></returns>
        Task InitAsync(IMapOptions mapOptions);

        /// <summary>
        /// 맵을 닫는다
        /// </summary>
        /// <returns></returns>
        Task CloseAsync();

        /// <summary>
        /// 사용자가 선택한 위치를 가져온다
        /// </summary>
        /// <returns>사용자가 선택한 위치</returns>
        Task<MapPoint> GetSelectedLocationAsync();
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
        }


        private const string JsFile = "/js/naver_map.js";
        private const string InitId = "init";
        private const string CloseId = "close";
        private const string GetMarkerLocationId = "getMarkerLocation";
        

        private readonly IJSRuntime _jsRuntime;
        private readonly DotNetObjectReference<NaverMapService> _dotNetRef;

        private IJSObjectReference? _module;


        public NaverMapService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            _dotNetRef = DotNetObjectReference.Create(this);
        }

        public EventCallback<MapPoint> LocationChanged { get; set; }
        public EventCallback<string> AddressChanged { get; set; }

        [JSInvokable]
        public void OnLocationChanged(double x, double y)
        {
            if (LocationChanged.HasDelegate)
                LocationChanged.InvokeAsync(new MapPoint() { X = x, Y = y });
        }

        [JSInvokable]
        public void OnAddressChanged(string address)
        {
            if (AddressChanged.HasDelegate)
                AddressChanged.InvokeAsync(address);
        }

        public async Task InitAsync(IMapOptions mapOptions)
        {
            _module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", JsFile);

            var naverMapOptions = (MapOptions)mapOptions;
            await _module.InvokeVoidAsync(InitId, _dotNetRef, naverMapOptions.MapId, naverMapOptions.CenterX, naverMapOptions.CenterY);
        }

        public async Task<MapPoint> GetSelectedLocationAsync()
        {
            if (_module == null)
                return new MapPoint();
            return await _module.InvokeAsync<MapPoint>(GetMarkerLocationId);
        }

        public async Task CloseAsync()
        {
            if (_module == null)
                return;
            await _module.InvokeVoidAsync(CloseId);
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
