using Microsoft.AspNetCore.Identity;

namespace Bridge.Infrastructure.Identity.Entities
{
    public class BridgeUser : IdentityUser<long>
    {
        public BridgeUser() { }

        /// <summary>
        /// 사용자 상세 정보
        /// </summary>
        public UserDetails UserDetails { get; set; } = new();

        /// <summary>
        /// 리프레시 토큰 정보
        /// </summary>
        public RefreshTokenDetails RefreshTokenDetails { get; set; } = new();
    
    }
}
