using Bridge.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Domain.Products.Exception
{
    /// <summary>
    /// 제품명이 유효하지 않을 때 발생하는 예외
    /// </summary>
    public class InvalidProductNameException : DomainException
    {
        public InvalidProductNameException() : base("제품명이 유효하지 않습니다")
        {
        }
    }
}
