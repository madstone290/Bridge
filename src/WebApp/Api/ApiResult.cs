using Bridge.Shared.ApiContract;
using System.Net;

namespace Bridge.WebApp.Api
{
    /// <summary>
    /// API 처리 결과
    /// </summary>
    public class ApiResult<TData> : IApiResult
    {
        protected ApiResult(bool success, TData? data, string? errorMessage = null, string? errorCode = null)
        {
            Success = success;
            Data = data;
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }

        /// <summary>
        /// 성공응답
        /// </summary>
        /// <returns></returns>
        public static ApiResult<TData> SuccessResult(TData? data) => new(true, data);

        /// <summary>
        /// 500에러
        /// </summary>
        /// <returns></returns>
        public static ApiResult<TData> ServerErrorResult() => new(false, default, "서버에서 오류가 발생하였습니다");

        /// <summary>
        /// 400에러
        /// </summary>
        /// <param name="error">400에러 컨텐츠</param>
        /// <returns></returns>
        public static ApiResult<TData> BadRequestResult(ErrorContent error) => new(false, default, error.Message, error.Code);

        /// <summary>
        /// 401권한없음 
        /// </summary>
        /// <returns></returns>
        public static ApiResult<TData> UnauthorizedResult() => new(false, default, "권한이 없습니다");

        /// <summary>
        /// 인증실패 오류
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static ApiResult<TData> AuthenticationErrorResult(string errorMessage) => new(false, default, $"인증에 실패하였습니다: {errorMessage}");

        /// <summary>
        /// 컨텐츠 파싱 에러
        /// </summary>
        /// <param name="content">컨텐츠 문자열</param>
        /// <returns></returns>
        public static ApiResult<TData> ContentParsingErrorResult(string content) => new(false, default, $"응답 컨텐츠 파싱에 실패하였습니다: {content}");

        /// <summary>
        /// 지원하지 않는 응답코드 수신 에러
        /// </summary>
        /// <param name="statusCode">응답코드</param>
        /// <returns></returns>
        public static ApiResult<TData> UnsupportedStatusCodeResult(HttpStatusCode statusCode) => new(false, default, $"지원하지 않는 상태코드입니다: {statusCode}");

        /// <summary>
        /// API 처리 성공여부
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// API 반환 데이터
        /// </summary>
        public TData? Data { get; }

        /// <summary>
        /// 에러 메시지
        /// </summary>
        public string? ErrorMessage { get; }

        /// <summary>
        /// 에러 코드
        /// </summary>
        public string? ErrorCode { get; }

    }

    /// <summary>
    /// 응답데이터가 없을 때 사용하는 결과 객체
    /// </summary>
    public class ApiResult : ApiResult<Void>
    {
        protected ApiResult(bool success, Void data, string? errorMessage = null, string? errorCode = null) : base(success, data, errorMessage, errorCode)
        {
        }
    }
}
