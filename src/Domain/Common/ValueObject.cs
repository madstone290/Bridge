using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Domain.Common
{
    public abstract class ValueObject
    {
        public static bool operator ==(ValueObject? left, ValueObject? right)
        {
            if (ReferenceEquals(left, right))
                return true;

            if (left is null)
                return right is null;

            return left.Equals(right);
        }

        public static bool operator !=(ValueObject? left, ValueObject? right)
        {
            return !(left == right);
        }

        /// <summary>
        /// 동일성 검사에 사용될 속성값 목록
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<object?> GetEqualityPropertyValues();

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;

            return GetEqualityPropertyValues().SequenceEqual(other.GetEqualityPropertyValues());
        }

        public override int GetHashCode()
        {
            return GetEqualityPropertyValues()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }
    }
}
