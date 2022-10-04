using Bridge.Application.Common;
using Bridge.Application.Common.Services;
using Microsoft.AspNetCore.Identity;
using Bridge.Shared.Extensions;
using Bridge.Infrastructure.Identity.Services;
using Bridge.Infrastructure.Identity.Entities;
using System.Security.Claims;

namespace Bridge.Infrastructure.Identity
{
    public class UserService
    {
        private readonly UserManager<BridgeUser> _userManager;
        private readonly IMailService _mailService;
        private readonly IClaimService _claimService;
        private readonly ITokenService _tokenService;
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly IAdminUserService _adminUserService;

        public UserService(UserManager<BridgeUser> userManager, IMailService mailService, IClaimService claimService, ITokenService tokenService, IEmailVerificationService emailVerificationService, IAdminUserService adminUserService)
        {
            _userManager = userManager;
            _mailService = mailService;
            _claimService = claimService;
            _tokenService = tokenService;
            _emailVerificationService = emailVerificationService;
            _adminUserService = adminUserService;
        }

        private static string IdentityErrorToString(IEnumerable<IdentityError> errors)
        {
            return string.Join("", errors.Select(x => x.Description));
        }

        /// <summary>
        /// 사용자를 생성한다.
        /// </summary>
        /// <param name="userDto">사용자 입력 데이터</param>
        /// <returns></returns>
        /// <exception cref="AppException"></exception>
        public async Task<long> RegisterAsync(string email, string password, string userName)
        {
            if(await _userManager.FindByEmailAsync(email) != null)
                throw new AppException("이미 등록된 이메일입니다", new { email });

            var userType = _adminUserService.VerifyAdmin(email) ? UserType.Admin : UserType.Consumer;
            var user = new BridgeUser()
            {
                UserName = email,
                Email = email,
                EmailConfirmed = _emailVerificationService.Verify(email),
                UserDetails = UserDetails.NewUserDetails(userName, userType)
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new AppException(IdentityErrorToString(result.Errors));
            return user.Id;
        }

        /// <summary>
        /// 인증메일을 전송한다
        /// </summary>
        /// <param name="email">메일주소</param>
        /// <param name="redirectUri">인증 완료 후 이동할 페이지 Uri</param>
        /// <param name="callbackUri">인증을 위한 콜백Uri</param>
        /// <returns></returns>
        /// <exception cref="AppException"></exception>
        public async Task SendVerificationEmailAsync(string email,string redirectUri, string callbackUri)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new AppException("사용자를 찾을 수 없습니다", new { email });
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            
            var callbackUriWithQuery = callbackUri
                .AddQueryParam("redirectUri", Uri.EscapeDataString(redirectUri))
                .AddQueryParam("token", Uri.EscapeDataString(token))
                .AddQueryParam("email", Uri.EscapeDataString(user.Email));

            var subject = "브릿지 이메일 인증";
            string body = $"<a href=\"{callbackUriWithQuery}\">브릿지 회원 가입을 환영합니다. 링크를 눌러 회원 가입을 완료하세요.</a>";
            await _mailService.SendAsync(email,  subject, body);
        }

        public async Task VerifyEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new AppException("사용자를 찾을 수 없습니다", new { email });
            
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if(!result.Succeeded)
                throw new AppException(IdentityErrorToString(result.Errors));
        }


        public async Task<RefreshResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || await _userManager.CheckPasswordAsync(user, password) == false)
                throw new AppException("로그인 정보가 유효하지 않습니다");

            if (!user.EmailConfirmed)
                throw new AppException("이메일 인증이 필요합니다");

            var claims = await _claimService.GetClaimsAsync(user);
            var accessToken = _tokenService.GenenateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshTokenDetails = RefreshTokenDetails.NewRefreshToken(refreshToken);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new AppException(IdentityErrorToString(result.Errors));

            return new RefreshResult()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public async Task<RefreshResult> RefreshAsync(string email, string refreshToken)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new AppException("사용자를 찾을 수 없습니다", new { email });

            if (user.RefreshTokenDetails == null || user.RefreshTokenDetails.ValidateRefreshToken(refreshToken) == false)
                throw new AppException("리프레시 토큰이 유효하지 않습니다");
            
            var claims = await _claimService.GetClaimsAsync(user);
            var newAccessToken = _tokenService.GenenateAccessToken(claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshTokenDetails = RefreshTokenDetails.NewRefreshToken(newRefreshToken);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new AppException(IdentityErrorToString(result.Errors));

            return new RefreshResult()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            };
        }


    }
}
