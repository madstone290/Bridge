namespace Bridge.Application.Common.Exceptions
{
    public class NoPermissionException : AppException
    {
        public NoPermissionException() : base("권한이 없습니다")
        {
        }
    }
}
