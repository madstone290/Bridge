using Bridge.Shared.ApiContract;
using Bridge.Shared.Extensions;
using Bridge.WebApp.Services.Identity;

namespace Bridge.WebApp.Api
{
    /// <summary>
    /// API 서버 통신에 사용되는 클라이언트
    /// </summary>
    public abstract class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService? _authService;

        /// <summary>
        /// 요청 전송시 인증이 적용되지 않는다
        /// </summary>
        /// <param name="httpClient"></param>
        protected ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// 요청 전송시 인증이 적용된다
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="authService"></param>
        protected ApiClient(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        /// <summary>
        /// 응답코드에 맞는 결과객체를 생성한다.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        private static async Task<ApiResult<TData>> BuildResultAsync<TData>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    if (typeof(TData) == typeof(Void))
                    {
                        return ApiResult<TData>.SuccessResult(default);
                    }
                    else
                    {
                        var data = await response.Content.ReadFromJsonAsync<TData>();
                        return ApiResult<TData>.SuccessResult(data);
                    }
                }
                catch
                {
                    var responseContentString = await response.Content.ReadAsStringAsync();
                    return ApiResult<TData>.ContentParsingErrorResult(responseContentString);
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return ApiResult<TData>.ServerErrorResult();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                try
                {

                    var errorContent = await response.Content.ReadFromJsonAsync<ErrorContent>() ?? null!;
                    return ApiResult<TData>.BadRequestResult(errorContent);
                }
                catch
                {
                    var responseContentString = await response.Content.ReadAsStringAsync();
                    return ApiResult<TData>.ContentParsingErrorResult(responseContentString);
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return ApiResult<TData>.UnauthorizedResult();
            }
            else
            {
                return ApiResult<TData>.UnsupportedStatusCodeResult(response.StatusCode);
            }
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
        virtual protected async Task<ApiResult<TData>> SendAsync<TData>(HttpMethod method, string uri, object? content = null)
        {
            var request = new HttpRequestMessage(method, uri);
            if (content != null)
                request.Content = JsonContent.Create(content);

            if(_authService == null)
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

                // 액세스 토큰이 만료된 경우 재발급 후 1회 재시도
                if (authState.IsAuthenticated && response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var refreshResult = await _authService.RefreshAsync();
                    if (!refreshResult.Success)
                        return ApiResult<TData>.AuthenticationErrorResult(refreshResult.Error);

                    var requestClone = await request.CloneAsync();
                    authState = await _authService.GetAuthStateAsync();
                    requestClone.SetBearerToken(authState.AccessToken);

                    response = await _httpClient.SendAsync(requestClone);
                }

                return await BuildResultAsync<TData>(response);
            }
        }

    }
}
