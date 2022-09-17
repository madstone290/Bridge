using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Domain.Common.Exceptions
{
    /// <summary>
    /// 위치가 유효하지 않을 경우 발생하는 예외
    /// </summary>
    public class InvalidLocationException : DomainException
    {
        public InvalidLocationException() : base("위치가 유효하지 않습니다")
        {
        }
    }
}
