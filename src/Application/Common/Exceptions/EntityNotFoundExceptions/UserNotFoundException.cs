using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Application.Common.Exceptions.EntityNotFoundExceptions
{
    public class UserNotFoundException : EntityNotFoundException
    {
        public UserNotFoundException(object? tag = null) : base("사용자를 찾을 수 없습니다", tag)
        {
        }
    }
}
