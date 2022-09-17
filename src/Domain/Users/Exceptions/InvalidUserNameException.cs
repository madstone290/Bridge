using Bridge.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Domain.Users.Exceptions
{
    /// <summary>
    /// 사용자 이름이 유효하지 않을 때 발생하는 예외
    /// </summary>
    public class InvalidUserNameException : DomainException
    {
        public InvalidUserNameException() : base("이름이 유효하지 않습니다")
        {
        }
    }
}
