using Bridge.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Domain.Common.Exceptions
{
    /// <summary>
    /// 장소를 생성하는 사용자의 권한이 없을 경우 발생하는 예외
    /// </summary>
    public class NoPermissionException : DomainException
    {
        public NoPermissionException() : base("사용자 권한이 없습니다")
        {
        }
    }
}
