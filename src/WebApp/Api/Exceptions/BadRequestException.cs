using Bridge.Shared.ApiContract;

namespace Bridge.WebApp.Api.Exceptions
{
    /// <summary>
    /// 400 응답을 받은 경우 발생
    /// </summary>
    public class BadRequestException : Exception
    {
        public ErrorContent ErrorContent { get; }
        public BadRequestException(ErrorContent errorContent) : base(errorContent.Message)
        {
            ErrorContent = errorContent;
        }
    }
}
