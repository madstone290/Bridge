using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Application.Common.Exceptions.EntityNotFoundExceptions
{
    public class ProductNotFoundException : EntityNotFoundException
    {
        public ProductNotFoundException(object? tag = null) : base("제품을 찾을 수 없습니다", tag)
        {
        }
    }
}
