using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Domain.Common
{
    /// <summary>
    /// 애그리게이트 루트
    /// </summary>
    public class AggregateRoot : Entity, IAggregateRoot
    {
        /// <summary>
        /// 소유자 아이디
        /// </summary>
        public string OwnerId { get; private set; } = string.Empty;
    }
}
