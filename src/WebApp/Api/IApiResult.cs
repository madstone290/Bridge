namespace Bridge.WebApp.Api
{
    public interface IApiResult
    {
        /// <summary>
        /// 성공 여부
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// 에러 메시지
        /// </summary>
        public string? ErrorMessage { get; }
    }
}
