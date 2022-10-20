using Bridge.Shared.ApiContract;

namespace Bridge.WebApp.Api
{

    /// <summary>
    /// API서버에 HTTP요청을 전송한다.
    /// </summary>
    public abstract class ApiClient
    {
        public readonly HttpClient HttpClient;

        protected ApiClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }


        /// <summary>
        /// 응답코드에 맞는 결과객체를 생성한다.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        protected static async Task<ApiResult<TData>> BuildResultAsync<TData>(HttpResponseMessage response)
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
                    var apiError = await response.Content.ReadFromJsonAsync<ApiError>()!;
                    if (apiError == null || string.IsNullOrWhiteSpace(apiError.Message))
                        return ApiResult<TData>.BadRequestResult(await response.Content.ReadAsStringAsync());
                    else
                        return ApiResult<TData>.BadRequestResult(apiError);
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
        ///  Http요청을 전송한다.
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

            var response = await HttpClient.SendAsync(request);

            return await BuildResultAsync<TData>(response);
        }

    }
}
