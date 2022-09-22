using System.Net;

namespace Bridge.WebApp.Api.Exceptions
{
    /// <summary>
    /// 지원하지 않는 상태코드를 수신하는 경우 발생
    /// </summary>
    public class UnsupportedStatusCodeExpception : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public UnsupportedStatusCodeExpception(HttpStatusCode statusCode) : base($"지원하지 않는 상태코드입니다: {statusCode}")
        {
            StatusCode = statusCode;
        }
    }
}
