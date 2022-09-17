using Bridge.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Domain.Products.Exception
{
    public class InvalidPriceException : DomainException
    {
        public InvalidPriceException() : base("제품 가격이 유효하지 않습니다")
        {
        }
    }
}
