using Bridge.Shared;

namespace Bridge.WebApp.Services.GeoLocation
{
    /// <summary>
    /// HTML Geolocation 서비스
    /// </summary>
    public interface IHtmlGeoService
    {
        /// <summary>
        /// 현재 위치를 확인한다. 
        /// </summary>
        /// <returns></returns>
        Task<HtmlGeoResult> GetLocationAsync();
    }

}
