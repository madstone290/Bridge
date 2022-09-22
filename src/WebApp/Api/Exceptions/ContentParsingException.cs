namespace Bridge.WebApp.Api.Exceptions
{
    /// <summary>
    /// 응답 컨텐츠 파싱이 실패하는 경우 발생
    /// </summary>
    public class ContentParsingException : Exception
    {
        public ContentParsingException(HttpContent httpContent) : base("응답 컨텐츠 파싱에 실패하였습니다")
        {
            HttpContent = httpContent;
        }

        public HttpContent HttpContent { get; }

        public string HttpContentString => HttpContent.ReadAsStringAsync().GetAwaiter().GetResult();

    }
}
