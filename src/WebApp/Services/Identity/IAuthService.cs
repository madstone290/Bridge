using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Bridge.WebApp.Api.ApiClients.Identity;
using Bridge.WebApp.Constants;
using Bridge.Shared.Constants;
using Bridge.Shared.ApiContract.Dtos.Identity;

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
        /// 리프레시토큰을 이용해 상태를 갱신한다.
        /// </summary>
        /// <returns></returns>
        Task<AuthResult> RefreshAsync(string refreshToken);

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

        public static AuthState Authenticated(string email, string userId, string userType, string accessToken, string refreshToken)
        {
            return new AuthState()
            {
                Email = email,
                UserId = userId,
                UserType = userType,
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
        /// 사용자 아이디
        /// </summary>
        public string UserId { get; init; } = string.Empty;

        /// <summary>
        /// 사용자 타입
        /// </summary>
        public string UserType { get; init; } = string.Empty;

        /// <summary>
        /// 액세스 토큰
        /// </summary>
        public string AccessToken { get; init; } = string.Empty;

        /// <summary>
        /// 리프레시 토큰
        /// </summary>
        public string RefreshToken { get; init; } = string.Empty;

        /// <summary>
        /// 토큰이 갱신된 새로운 인증 상태 인스턴스를 반환한다.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public AuthState Refresh(string accessToken, string refreshToken)
        {
            return Authenticated(Email, UserId, UserType, accessToken, refreshToken);
        }
    }

    public class AuthService : AuthenticationStateProvider, IAuthService
    {
        private readonly SemaphoreSlim _semaphore = new(1, 1);
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
                new Claim(ClaimTypeConstants.UserType, authState.UserType),
            };
            var identity = new ClaimsIdentity(cliams, AuthConstants.SessionCookieScheme);
            return new ClaimsPrincipal(identity);
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            var apiResult = await _userApiClient.LoginAsync(new LoginDto()
            {
                Email = email,
                Password = password
            });

            if (!apiResult.Success)
                return new AuthResult() { Success = false, Error = apiResult.Error ?? string.Empty };
            if (apiResult.Data == null)
                return new AuthResult() { Success = false, Error = "결과 데이터가 없습니다" };

            var loginResult = apiResult.Data;
            var authState = AuthState.Authenticated(email, loginResult.UserId, loginResult.UserType, loginResult.AccessToken, loginResult.RefreshToken);
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

        public async Task<AuthResult> RefreshAsync(string refreshToken)
        {
            /*
             * 여러개의 비동기 요청으로 토큰갱신이 동시에 발생할 수 있다.
             * 현재 리프레시 토큰과 갱신요청에 주어진 리프레시 토큰을 비교해서 값이 다른 경우 다른 스레드에서 갱신이 완료되었다고 판단한다.
             * */
            await _semaphore.WaitAsync();

            var authState = await GetAuthStateAsync();
            if (authState.RefreshToken != refreshToken) // 다른 스레드에 의해 토큰이 갱신된 경우
            {
                _semaphore.Release();
                return new AuthResult() { Success = true };
            }

            var apiResult = await _userApiClient.RefreshAsync(new RefreshDto()
            {
                Email = authState.Email,
                RefreshToken = authState.RefreshToken
            });

            if (!apiResult.Success)
                return new AuthResult() { Success = false, Error = apiResult.Error ?? string.Empty };
            if (apiResult.Data == null)
                return new AuthResult() { Success = false, Error = "결과 데이터가 없습니다" };

            var refreshResult = apiResult.Data;
            authState = authState.Refresh(refreshResult.AccessToken, refreshResult.RefreshToken);
            await _cookieService.SetCookieAsync(LocalStorageKeyConstants.AuthState, authState);

            var principal = GetPrincipalFromAuthState(authState);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));

            _semaphore.Release();
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
