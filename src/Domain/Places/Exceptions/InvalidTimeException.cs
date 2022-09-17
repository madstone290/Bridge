using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge.Domain.Common;

namespace Bridge.Domain.Places.Exceptions
{
    /// <summary>
    /// 시간이 유효하지 않은 경우 발생하는 예외
    /// </summary>
    public class InvalidTimeException : DomainException
    {
        public InvalidTimeException() : base("시간이 유효하지 않습니다")
        {
        }
    }
}
