using Bridge.Domain.Common;

namespace Bridge.Domain.Places.Exceptions
{
    /// <summary>
    /// 위치가 유효하지 않을 경우 발생하는 예외
    /// </summary>
    public class InvalidPlaceLocationException : DomainException
    {
        public InvalidPlaceLocationException() : base("위치가 유효하지 않습니다")
        {
        }
    }
}
