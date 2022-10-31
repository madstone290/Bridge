using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

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
        /// 선택한 마커가 변경되는 경우 호출할 콜백을 등록한다.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="callback">마커ID를 파라미터로 갖는 콜백</param>
        void SetOnSelectedMarkerChangedCallback(string sessionId, EventCallback<string> callback);

        /// <summary>
        /// 맵을 초기화한다
        /// </summary>
        /// <param name="sessionId">세션 아이디. 서비스에서 맵 식별을 위해 사용한다.</param>
        /// <param name="mapOptions">맵 옵션</param>
        /// <returns></returns>
        Task InitAsync(string sessionId, IMapOptions mapOptions);

        /// <summary>
        /// 마커를 추가한다
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="markers"></param>
        /// <returns></returns>
        Task AddMarkersAsync(string sessionId, IEnumerable<Marker> markers);

        /// <summary>
        /// 마커를 제거한다
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        Task ClearMarkersAsync(string sessionId);

        /// <summary>
        /// 마커를 선택한다
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="markerId"></param>
        /// <returns></returns>
        Task SelectMarkerAsync(string sessionId, string markerId);

        /// <summary>
        /// 해당 위치로 이동한다
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        Task MoveAsync(string sessionId, double latitude, double longitude);

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

}
