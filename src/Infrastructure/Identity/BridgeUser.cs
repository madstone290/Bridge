using Microsoft.AspNetCore.Identity;

namespace Bridge.Infrastructure.Identity
{
    public class BridgeUser : IdentityUser<long>
    {
        private BridgeUser() { }

        /// <summary>
        /// 사용자 상세정보
        /// </summary>
        public UserDetails UserDetails { get; set; } = new();   
    }
}
