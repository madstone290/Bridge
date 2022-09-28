namespace Bridge.Infrastructure.Identity.Entities
{
    /// <summary>
    /// 리프레시 토큰
    /// </summary>
    public class RefreshTokenDetails
    {
        public static RefreshTokenDetails NewRefreshToken(string token) => new()
        {
            Value = token,
            UtcExpires = DateTime.UtcNow.AddDays(3)
        };

        /// <summary>
        /// 토큰 값
        /// </summary>
        public string Value { get; private set; } = string.Empty;

        /// <summary>
        /// UTC 만료시간
        /// </summary>
        public DateTime UtcExpires { get; private set; }

        /// <summary>
        /// 만료 여부
        /// </summary>
        public bool IsExpired => UtcExpires <= DateTime.UtcNow;

        public bool ValidateRefreshToken(string refreshToken)
        {
            if (IsExpired)
                return false;
            return Value == refreshToken;
        }
    }
}
