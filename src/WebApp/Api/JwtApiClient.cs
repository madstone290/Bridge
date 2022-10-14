using Bridge.Shared.ApiContract;
using Bridge.Shared.Extensions;
using Bridge.WebApp.Services.Identity;

namespace Bridge.WebApp.Api
{
    /// <summary>
    /// API서버에 HTTP요청을 전송한다. 요청시 JWT토큰을 추가한다.
    /// </summary>
    public abstract class JwtApiClient : ApiClient
    {
        protected readonly IAuthService _authService;

        public bool AddJwtToken { get; protected set; } = true;

        protected JwtApiClient(HttpClient httpClient, IAuthService authService) : base(httpClient)
        {
            _authService = authService;
        }

        /// <summary>
        ///  Http요청을 전송한다. 인증여부에 따라 JWT 토큰을 추가한다.
        ///  <para>JWT토큰이 만료된 경우 재발급 후 1회 재시도 한다.</para>
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="method"></param>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        override protected async Task<ApiResult<TData>> SendAsync<TData>(HttpMethod method, string uri, object? content = null)
        {
            var request = new HttpRequestMessage(method, uri);
            if (content != null)
                request.Content = JsonContent.Create(content);

            if (!AddJwtToken)
            {
                var response = await _httpClient.SendAsync(request);
                return await BuildResultAsync<TData>(response);
            }
            else
            {
                var authState = await _authService.GetAuthStateAsync();
                if (authState.IsAuthenticated)
                    request.SetBearerToken(authState.AccessToken);

                var response = await _httpClient.SendAsync(request);

                if(response.StatusCode == System.Net.HttpStatusCode.Found || response.StatusCode == System.Net.HttpStatusCode.TemporaryRedirect)
                {
                    request = await request.CloneAsync();
                    request.RequestUri = response.Headers.Location;
                    authState = await _authService.GetAuthStateAsync();
                    request.SetBearerToken(authState.AccessToken);
                    response = await _httpClient.SendAsync(request);
                }

                // 액세스 토큰이 만료된 경우 재발급 후 1회 재시도
                if (authState.IsAuthenticated && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var refreshResult = await _authService.RefreshAsync();
                    if (!refreshResult.Success)
                        return ApiResult<TData>.AuthenticationErrorResult(refreshResult.Error);

                    request = await request.CloneAsync();
                    authState = await _authService.GetAuthStateAsync();
                    request.SetBearerToken(authState.AccessToken);

                    response = await _httpClient.SendAsync(request);
                }

                return await BuildResultAsync<TData>(response);
            }
        }

    }
}
