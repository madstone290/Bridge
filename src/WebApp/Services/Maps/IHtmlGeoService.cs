using Bridge.Shared;
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
        /// 현재 위치를 확인한다. 
        /// </summary>
        /// <returns></returns>
        Task<HtmlGeoResult> GetLocationAsync();
    }

    public class HtmlGeoResult : Result<GeoPoint>
    {
        public HtmlGeoResult(GeoPoint point)
        {
            Data = point;
            Success = true;
        }

        public HtmlGeoResult(GeoError geoError)
        {
            GeoError = geoError;
            Error = geoError.FriendlyMessage;
            Success = false;
        }

        public HtmlGeoResult(string error)
        {
            Error = error;
            Success = false;
        }

        public GeoError? GeoError { get; }
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

        public double Latitude { get; }
        public double Longitude { get; }
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
            FriendlyMessage = code switch
            {
                1 => "위치 권한이 없습니다",
                2 => "내부 오류로 위치를 확인할 수 없습니다",
                3 => "위치 확인 중 타임아웃이 발생하였습니다",
                _ => "알수없는 응답코드입니다"
            };
        }

        public int Code { get; }
        public string Message { get; }
        public string FriendlyMessage { get; }
    }

    public class HtmlGeoService : IHtmlGeoService
    {
        private const string JsFile = "/js/geo.js";
        private const string SetDotNetRefId = "setDotNetRef";
        private const string GetLocationId = "getLocation";

        private readonly IJSRuntime _jsRuntime;

        private DotNetObjectReference<HtmlGeoService>? _objRef;
        private IJSObjectReference? _module;

        private TaskCompletionSource? _taskCompletionSource;

        private GeoPoint? _geoPoint;
        private GeoError? _geoError;

        public HtmlGeoService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<HtmlGeoResult> GetLocationAsync()
        {
            if (_module == null)
            {
                _module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", JsFile);
                _objRef = DotNetObjectReference.Create(this);

                await _module.InvokeVoidAsync(SetDotNetRefId, _objRef);
            }

            _geoPoint = null;
            _geoError = null;
            _taskCompletionSource = new TaskCompletionSource();

            await _module.InvokeVoidAsync(GetLocationId);
            await _taskCompletionSource.Task;

            if (_geoPoint != null)
                return new HtmlGeoResult(_geoPoint);
            else if (_geoError != null)
                return new HtmlGeoResult(_geoError);
            else
                return new HtmlGeoResult("Geolocation Api 호출 중 타임아웃이 발생하였습니다");
        }

        /// <summary>
        /// 위치 조회에 성공한 경우 호출된다.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        [JSInvokable]
        public void OnSuccess(double latitude, double longitude)
        {
            _geoPoint = new GeoPoint(latitude, longitude);
            _taskCompletionSource?.TrySetResult();
        }

        /// <summary>
        /// 위치 조회중 에러 발생시 호출된다.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        [JSInvokable]
        public void OnError(int code, string message)
        {
            _geoError = new GeoError(code, message);
            _taskCompletionSource?.TrySetResult();
        }


    }
}
