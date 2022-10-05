
namespace Bridge.Shared.Extensions
{
    /// <summary>
    /// Http 프로토콜에서 사용가능한 확장 메소드
    /// </summary>
    public static class HttpExtensions
    {
        private const string AuthorizationHeader = "Authorization";
        private const string BearerScheme = "bearer";

        /// <summary>
        /// Authorization헤더에 Bearer토큰값을 설정한다.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <param name="accessToken"></param>
        public static void SetBearerToken(this HttpRequestMessage requestMessage, string accessToken)
        {
            requestMessage.Headers.Remove(AuthorizationHeader);
            requestMessage.Headers.Add(AuthorizationHeader, $"{BearerScheme} {accessToken}");
        }

        /// <summary>
        /// HttpRequestMessage를 복사한다.
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public static async Task<HttpRequestMessage> CloneAsync(this HttpRequestMessage requestMessage)
        {
            var clone = new HttpRequestMessage(requestMessage.Method, requestMessage.RequestUri);
            clone.Version = requestMessage.Version;
            if (requestMessage.Content != null)
                clone.Content = await requestMessage.Content.CloneAsync();

            foreach (var option in requestMessage.Options)
            {
                clone.Options.TryAdd(option.Key, option.Value);
            }
            foreach (KeyValuePair<string, IEnumerable<string>> header in requestMessage.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }

        /// <summary>
        /// HttpContent를 복사한다.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task<HttpContent> CloneAsync(this HttpContent content)
        {
            var ms = new MemoryStream();
            await content.CopyToAsync(ms);
            ms.Position = 0;

            var clone = new StreamContent(ms);
            foreach (KeyValuePair<string, IEnumerable<string>> header in content.Headers)
            {
                clone.Headers.Add(header.Key, header.Value);
            }
            return clone;
        }

    }
}
