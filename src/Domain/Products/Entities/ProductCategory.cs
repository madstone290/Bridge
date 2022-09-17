using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Domain.Products.Entities
{
    /// <summary>
    /// 제품 카테고리
    /// </summary>
    public enum ProductCategory
    {
        /// <summary>
        /// 음료
        /// </summary>
        Beverage,

        /// <summary>
        /// 식품
        /// </summary>
        Food,

        /// <summary>
        /// 비건 음료
        /// </summary>
        VeganBeverage,

        /// <summary>
        /// 비건 식품
        /// </summary>
        VeganFood,

        /// <summary>
        /// 문구
        /// </summary>
        Stationery


    }
}
