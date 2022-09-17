using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Domain.Common
{
    /// <summary>
    /// 도메인 엔티티
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// 엔티티 아이디
        /// </summary>
        public long Id { get; private set; }
    }
}
