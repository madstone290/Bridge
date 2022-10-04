namespace Bridge.Infrastructure.Identity
{
    /// <summary>
    /// 로그인 후 반환되는 데이터
    /// </summary>
    public class LoginResult
    {
        /// <summary>
        /// 사용자 타입
        /// </summary>
        public string UserType { get; set; } = string.Empty;

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
