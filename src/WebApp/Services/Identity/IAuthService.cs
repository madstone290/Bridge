using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Bridge.WebApp.Api.ApiClients.Identity;
using Bridge.Api.Controllers.Identity.Dtos;
using Bridge.WebApp.Constants;

namespace Bridge.WebApp.Services.Identity
{
    /// <summary>
    /// 사용자 인증 서비스
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// 로그인 진행
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<AuthResult> LoginAsync(string email, string password);

        /// <summary>
        /// 로그아웃 진행
        /// </summary>
        /// <returns></returns>
        Task<AuthResult> LogoutAsync();

        /// <summary>
        /// 인증상태 조회
        /// </summary>
        /// <returns></returns>
        Task<AuthState> GetAuthStateAsync();
    }

    /// <summary>
    /// 인증행위에 대한 결과
    /// </summary>
    public class AuthResult
    {
        /// <summary>
        /// 성공 여부
        /// </summary>
        public bool Success { get; init; }

        /// <summary>
        /// 에러메시지
        /// </summary>
        public string Error { get; init; } = string.Empty;
    }

    /// <summary>
    /// 인증 상태
    /// </summary>
    public class AuthState
    {
        public static AuthState Unauthenticated()
        {
            return new AuthState()
            {
                IsAuthenticated = false
            };
        }

        public static AuthState Authenticated(string email, string accessToken, string refreshToken)
        {
            return new AuthState()
            {
                Email = email,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                IsAuthenticated = true
            };
        }

        /// <summary>
        /// 인증 여부
        /// </summary>
        public bool IsAuthenticated { get; init; }

        /// <summary>
        /// 사용자 이메일
        /// </summary>
        public string Email { get; init; } = string.Empty;

        /// <summary>
        /// 액세스 토큰
        /// </summary>
        public string AccessToken { get; init; } = string.Empty;

        /// <summary>
        /// 리프레시 토큰
        /// </summary>
        public string RefreshToken { get; init; } = string.Empty;
    }

    public class AuthService : AuthenticationStateProvider, IAuthService
    {
        private readonly UserApiClient _userApiClient;
        private readonly ICookieService _cookieService;

        public AuthService(UserApiClient userApiClient, ICookieService cookieService)
        {
            _userApiClient = userApiClient;
            _cookieService = cookieService;
        }

        private static ClaimsPrincipal GetPrincipalFromAuthState(AuthState authState)
        {
            if(!authState.IsAuthenticated)
                return new ClaimsPrincipal();

            var cliams = new List<Claim>()
            {
                new Claim("Email", authState.Email),
                new Claim("AccessToken",  authState.AccessToken),
                new Claim("RefreshToken", authState.RefreshToken),
            };
            var identity = new ClaimsIdentity(cliams, AuthConstants.SessionCookieScheme);
            return new ClaimsPrincipal(identity);
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            var loginResult = await _userApiClient.LoginAsync(new LoginDto()
            {
                Email = email,
                Password = password
            });

            if (!loginResult.Success)
                return new AuthResult() { Success = false, Error = loginResult.ErrorMessage ?? string.Empty };
            if (loginResult.Data == null)
                return new AuthResult() { Success = false, Error = "토큰 데이터가 없습니다" };

            var authState = AuthState.Authenticated(email, loginResult.Data.AccessToken, loginResult.Data.RefreshToken);
            await _cookieService.SetCookieAsync(LocalStorageKeyConstants.AuthState, authState);

            var principal = GetPrincipalFromAuthState(authState);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));

            return new AuthResult() { Success = true };
        }

        public async Task<AuthResult> LogoutAsync()
        {
            await _cookieService.SetCookieAsync(LocalStorageKeyConstants.AuthState, string.Empty, -1);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));

            return new AuthResult() { Success = true };
        }

        public async Task<AuthState> GetAuthStateAsync()
        {
            return await _cookieService.GetCookieAsync<AuthState>(LocalStorageKeyConstants.AuthState) ?? AuthState.Unauthenticated();
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var authState = await GetAuthStateAsync();
            var principal = GetPrincipalFromAuthState(authState);
            return new AuthenticationState(principal);
        }

    }
}
