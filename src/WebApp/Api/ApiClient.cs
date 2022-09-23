using Bridge.Shared.ApiContract;
using System.Xml.Linq;

namespace Bridge.WebApp.Api
{
    public abstract class ApiClient
    {
        private readonly HttpClient _httpClient;

        protected ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        virtual protected async Task<ApiResult<TData>> SendAsync<TData>(HttpMethod method, string uri, object? content = null)
        {
            var request = new HttpRequestMessage(method, uri);
            if (content != null)
                request.Content = JsonContent.Create(content);

            var response = await _httpClient.SendAsync(request);

            return await BuildResultAsync<TData>(response);

        }

        /// <summary>
        /// 응답코드에 맞는 결과객체를 생성한다.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task<ApiResult<TData>> BuildResultAsync<TData>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var data = await response.Content.ReadFromJsonAsync<TData>();
                    return ApiResult<TData>.SuccessResult(data);
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
            else
            {
                return ApiResult<TData>.UnsupportedStatusCodeResult(response.StatusCode);
            }
        }


    }
}
