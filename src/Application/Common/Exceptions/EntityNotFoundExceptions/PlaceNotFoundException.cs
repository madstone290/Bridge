using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Application.Common.Exceptions.EntityNotFoundExceptions
{
    public class PlaceNotFoundException : EntityNotFoundException
    {
        public PlaceNotFoundException(object? tag = null) : base("장소를 찾을 수 없습니다", tag)
        {
        }
    }
}
