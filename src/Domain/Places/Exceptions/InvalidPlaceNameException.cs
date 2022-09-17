using Bridge.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Domain.Places.Exceptions
{
    /// <summary>
    /// 장소명이 유효하지 않을 때 발생하는 예외
    /// </summary>
    public class InvalidPlaceNameException : DomainException
    {
        public InvalidPlaceNameException() : base("장소명이 유효하지 않습니다")
        {
        }
    }
}
