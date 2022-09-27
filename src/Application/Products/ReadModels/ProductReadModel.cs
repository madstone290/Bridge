using Bridge.Domain.Places.Entities;
using Bridge.Domain.Products.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Application.Products.ReadModels
{
    public class ProductReadModel
    {
        /// <summary>
        /// 아이디
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 제품유형
        /// </summary>
        public ProductType Type { get; set; }

        /// <summary>
        /// 제품명
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 제품이 판매되는 장소
        /// </summary>
        public long PlaceId { get; set; }

        /// <summary>
        /// 제품 가격
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// 제품 카테고리
        /// </summary>
        public List<ProductCategory> Categories { get; set; } = new();
    }
}
