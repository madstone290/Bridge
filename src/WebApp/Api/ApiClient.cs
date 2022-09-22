using Bridge.Shared.ApiContract;
using Bridge.WebApp.Api.Exceptions;

namespace Bridge.WebApp.Api
{
    public abstract class ApiClient
    {
        private readonly HttpClient _httpClient;

        protected ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected async Task<TData> SendAsync<TData>(HttpMethod method, string uri, object? content = null)
        {
            var request = new HttpRequestMessage(method, uri);
            if (content != null)
                request.Content = JsonContent.Create(content);


            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<TData>() ?? default!;
                }
                catch
                {
                    throw new ContentParsingException(response.Content);
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new ServerErrorException();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                try
                {
                    var errorContent = await response.Content.ReadFromJsonAsync<ErrorContent>() ?? null!;
                    throw new BadRequestException(errorContent);
                }
                catch
                {
                    throw new ContentParsingException(response.Content);
                }
            }
            else
            {
                throw new UnsupportedStatusCodeExpception(response.StatusCode);
            }

        }
    }
}
