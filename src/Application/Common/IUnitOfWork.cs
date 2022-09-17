using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Application.Common
{
    /// <summary>
    /// 작업단위 관리 인터페이스.
    /// 작업단위는 커맨드 핸들러에서만 관리하도록 한다.
    /// 이벤트 핸들러에서 커밋하지 말 것.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 현재까지의 작업을 커밋한다.
        /// </summary>
        /// <returns></returns>
        Task CommitAsync();
    }
}
