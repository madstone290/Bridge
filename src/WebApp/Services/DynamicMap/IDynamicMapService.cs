using Microsoft.AspNetCore.Components;

namespace Bridge.WebApp.Services.DynamicMap
{
    /// <summary>
    /// 동적 지도 서비스
    /// </summary>
    public interface IDynamicMapService : IAsyncDisposable
    {
        /// <summary>
        /// 지도 중심위치가 변경될 경우 호출할 콜백을 등록한다.
        /// </summary>
        /// <param name="callback">이벤트 콜백</param>
        void SetCenterChangedCallback(EventCallback<MapPoint> callback);

        /// <summary>
        /// 선택한 마커가 변경되는 경우 호출할 콜백을 등록한다.
        /// </summary>
        /// <param name="callback">마커ID를 파라미터로 갖는 콜백</param>
        void SetOnSelectedMarkerChangedCallback(EventCallback<string> callback);

        /// <summary>
        /// 컨텍스트 메뉴가 클릭되는 경우 호출할 콜백을 등록한다.
        /// </summary>
        /// <param name="callback">메뉴ID/좌표를 파라미터로 갖는 콜백</param>
        void SetOnContextMenuClickedCallback(EventCallback<Tuple<string, MapPoint>> callback);

        /// <summary>
        /// 맵을 초기화한다
        /// </summary>
        /// <param name="mapOptions">맵 옵션</param>
        /// <returns></returns>
        Task InitAsync(IMapOptions mapOptions);

        /// <summary>
        /// 장소마커를 추가한다
        /// </summary>
        /// <param name="placeMarkers"></param>
        /// <returns></returns>
        Task AddPlaceMarkersAsync(IEnumerable<Marker> placeMarkers);

        /// <summary>
        /// 장소마커를 제거한다
        /// </summary>
        /// <returns></returns>
        Task ClearPlaceMarkersAsync();

        /// <summary>
        /// 장소마커를 선택한다
        /// </summary>
        /// <param name="markerId"></param>
        /// <returns></returns>
        Task SelectPlaceMarkerAsync(string markerId);

        /// <summary>
        /// 지도를 해당 위치로 이동한다
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        Task MoveAsync(double latitude, double longitude);

        /// <summary>
        /// 맵을 해제한다
        /// </summary>
        /// <returns></returns>
        Task DisposeMapAsync();

        /// <summary>
        /// 내 위치를 가져온다
        /// </summary>
        /// <returns>내 위치</returns>
        Task<MapPoint> GetMyLocationAsync();

        /// <summary>
        /// 내 위치를 설정한다
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        Task<MapPoint> SetMyLocationAsync(MapPoint location);


    }

}
