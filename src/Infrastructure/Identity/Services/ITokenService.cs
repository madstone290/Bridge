using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Bridge.Infrastructure.Identity.Services
{
    public interface ITokenService
    {
        /// <summary>
        /// 액세스 토큰을 생성한다.
        /// </summary>
        /// <param name="claims">토큰에 포함될 클레임 목록</param>
        /// <returns></returns>
        string GenenateAccessToken(IEnumerable<Claim> claims);

        /// <summary>
        /// 리프레시 토큰을 생성한다.
        /// </summary>
        /// <returns></returns>
        string GenerateRefreshToken();
    }

    public class TokenService : ITokenService
    {
        public class Config
        {
            /// <summary>
            /// 토큰 생성 키
            /// </summary>
            [Required]
            public string Key { get; set; } = string.Empty;

            /// <summary>
            /// 토큰 발행자
            /// </summary>
            public string Issuer { get; set; } = string.Empty;

            /// <summary>
            /// 토큰 수신자
            /// </summary>
            public string Audience { get; set; } = string.Empty;

            /// <summary>
            /// 액세스 토큰 만료시간 (분)
            /// </summary>
            [Range(10, 120)]
            public int ExpiryInMinutes { get; set; } = 60;
        }

        private readonly Config _config;

        public TokenService(IOptions<Config> configOptions)
        {
            _config = configOptions.Value;
        }

        public string GenenateAccessToken(IEnumerable<Claim> claims)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _config.Issuer,
                audience: _config.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_config.ExpiryInMinutes),
                signingCredentials: signingCredentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return accessToken;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
