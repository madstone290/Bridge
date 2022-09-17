using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Application.Common.Exceptions
{
    public class EntityNotFoundException : AppException
    {
        public EntityNotFoundException(string message, object? tag = null) : base(message, tag)
        {
        }
    }
}
