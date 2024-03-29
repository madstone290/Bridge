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

        public EventCallback<MapPoint> CenterChangedCallback { get; set; }
        public EventCallback<string> SelectedMarkerChangedCallback { get; set; }
        public EventCallback<MapPoint> ChangeMyLocationClickCallback { get; set; }
        public EventCallback<MapPoint> AddPlaceClickCallback { get; set; }

        public void SetCenterChangedCallback(EventCallback<MapPoint> callback)
        {
            CenterChangedCallback = callback;
        }

        public void SetOnSelectedMarkerChangedCallback(EventCallback<string> callback)
        {
            SelectedMarkerChangedCallback = callback;
        }

        /// <summary>
        /// 지도 중심이 변경된 경우
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        [JSInvokable]
        public void OnCenterChanged(double x, double y)
        {
            if(CenterChangedCallback.HasDelegate)
                CenterChangedCallback.InvokeAsync(new MapPoint() { X = x, Y = y });
        }

        /// <summary>
        /// 선택 마커가 변경된 경우
        /// </summary>
        /// <param name="markerId"></param>
        [JSInvokable]
        public void OnSelectedPlaceMarkerChanged(string markerId)
        {
            if (SelectedMarkerChangedCallback.HasDelegate)
                SelectedMarkerChangedCallback.InvokeAsync(markerId);
        }

        /// <summary>
        /// 내위치 변경 클릭
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        [JSInvokable]
        public void OnChangeMyLocationClick(double x, double y)
        {
            if (ChangeMyLocationClickCallback.HasDelegate)
                ChangeMyLocationClickCallback.InvokeAsync(new MapPoint() { X = x, Y = y });
        }

        /// <summary>
        /// 장소추가 클릭
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        [JSInvokable]
        public void OnAddPlaceClick(double x, double y)
        {
            if (AddPlaceClickCallback.HasDelegate)
                AddPlaceClickCallback.InvokeAsync(new MapPoint() { X = x, Y = y });
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
            if(_module != null)
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