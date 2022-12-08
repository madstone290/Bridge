using Bridge.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Bridge.Infrastructure.Identity.Services
{
    public interface IClaimService
    {
        /// <summary>
        /// 사용자의 클레임을 가져온다.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IEnumerable<Claim>> GetClaimsAsync(BridgeUser user);
    }

    public class ClaimService : IClaimService
    {
        private readonly UserManager<BridgeUser> _userManager;

        public ClaimService(UserManager<BridgeUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync(BridgeUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            claims.Add(new Claim("UserType", user.UserDetails.UserType.ToString()));
            claims.Add(new Claim("UserId", user.Id.ToString()));

            return claims;
        }
    }
}
