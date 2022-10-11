using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bridge.WebApp.Services.Maps
{
    /// <summary>
    /// HTML Geolocation 서비스
    /// </summary>
    public interface IHtmlGeoService
    {
        /// <summary>
        /// 위치 조회 성공시 호출할 콜백
        /// </summary>
        EventCallback<GeoPoint> SuccessCallback { get; set; }

        /// <summary>
        /// 위치 조회 에러 발생시 호출할 콜백
        /// </summary>
        EventCallback<GeoError> ErrorCallback { get; set; }

        /// <summary>
        /// 현재 위치를 확인한다. 확인 후 이벤트 콜백이 실행된다. 위치 확인이 불가능한 경우 null이 전달된다.
        /// </summary>
        /// <returns></returns>
        Task GetLocationAsync();
    }

    /// <summary>
    /// Geolocation api 위치
    /// </summary>
    public class GeoPoint
    {
        public GeoPoint(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; init; }
        public double Longitude { get; init; }
    }

    /// <summary>
    /// Geolocation api 에러
    /// </summary>
    public class GeoError
    {
        public GeoError(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; init; }
        public string Message { get; init; }
    }

    public class HtmlGeoService : IHtmlGeoService
    {
        private const string JsFile = "/js/geo.js";
        private const string SetDotNetRefId = "setDotNetRef";
        private const string GetLocationId = "getLocation";

        private readonly IJSRuntime _jsRuntime;

        private DotNetObjectReference<HtmlGeoService>? _objRef;
        private IJSObjectReference? _module;

        public EventCallback<GeoPoint> SuccessCallback { get; set; }

        public EventCallback<GeoError> ErrorCallback { get; set; }

        public HtmlGeoService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task GetLocationAsync()
        {
            if (_module == null)
            {
                _module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", JsFile);
                _objRef = DotNetObjectReference.Create(this);

                await _module.InvokeVoidAsync(SetDotNetRefId, _objRef);
            }
            await _module.InvokeVoidAsync(GetLocationId);
        }

        /// <summary>
        /// 위치 조회에 성공한 경우 호출된다.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        [JSInvokable]
        public void OnSuccess(double latitude, double longitude)
        {
            if (!SuccessCallback.HasDelegate)
                return;
            SuccessCallback.InvokeAsync(new GeoPoint(latitude, longitude));
        }

        /// <summary>
        /// 위치 조회중 에러 발생시 호출된다.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        [JSInvokable]
        public void OnError(int code, string message)
        {
            if (!ErrorCallback.HasDelegate)
                return;
            ErrorCallback.InvokeAsync(new GeoError(code, message));
        }


    }
}
