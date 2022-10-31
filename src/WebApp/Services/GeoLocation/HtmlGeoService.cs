using Microsoft.JSInterop;

namespace Bridge.WebApp.Services.GeoLocation
{
    public class HtmlGeoService : IHtmlGeoService
    {
        private const string JsFile = "/js/geo.js";
        private const string SetDotNetRefId = "setDotNetRef";
        private const string GetLocationId = "getLocation";
        private const int RequestTimeout = 2000;

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

            var timeout = Task.Delay(RequestTimeout);
            await _module.InvokeVoidAsync(GetLocationId);

            await Task.WhenAny(_taskCompletionSource.Task, timeout);

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
