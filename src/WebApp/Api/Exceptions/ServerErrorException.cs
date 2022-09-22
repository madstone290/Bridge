namespace Bridge.WebApp.Api.Exceptions
{
    /// <summary>
    /// 500 응답을 받은 경우 발생
    /// </summary>
    public class ServerErrorException : Exception
    {
        public ServerErrorException() : base("서버에서 오류가 발생하였습니다")
        {
        }
    }
}
