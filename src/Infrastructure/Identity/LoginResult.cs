namespace Bridge.Infrastructure.Identity
{
    /// <summary>
    /// 토큰 생성 후 반환되는 결과 데이터
    /// </summary>
    public class TokenResult
    {
        /// <summary>
        /// 액세스 토큰
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// 리프레시 토큰
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;
    }
}
