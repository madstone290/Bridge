namespace Bridge.Shared.ApiContract
{
    /// <summary>
    /// API 응답의 에러 포맷.
    /// </summary>
    public class ErrorContent
    {
        /// <summary>
        /// 에러 메시지
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 에러 코드
        /// </summary>
        public string? Code { get; set; }
    }
}
