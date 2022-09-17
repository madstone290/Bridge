using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Domain.Common
{
    public class DomainException : Exception
    {
        /// <summary>
        /// 에러 상세 데이터 객체
        /// </summary>
        public object? Tag { get; private set; }

        public DomainException(string? message, object? tag = null) : base(message)
        {
            Tag = tag;
        }
    }
}
