using Bridge.Application.Products.ReadModels;
using Bridge.Domain.Products.Entities;

namespace Bridge.WebApp.Pages.Admin.Models
{
    public class ProductModel
    {
        public static ProductModel Create(ProductReadModel x)
        {
            return new ProductModel()
            {
                Id = x.Id,
                Name = x.Name,
                PlaceId = x.PlaceId,
                Categories = x.Categories,
                Price = x.Price,
                Type = x.Type,
            };
        }

        /// <summary>
        /// 아이디
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 제품유형
        /// </summary>
        public ProductType Type { get; set; }

        /// <summary>
        /// 제품유형 문자열
        /// </summary>
        public string TypeString => Type.ToString();

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
        /// 제품 가격 문자열
        /// </summary>
        public string PriceString => Price?.ToString() ?? string.Empty;

        /// <summary>
        /// 제품 카테고리
        /// </summary>
        public List<ProductCategory> Categories { get; set; } = new();

        /// <summary>
        /// 카테고리 문자열
        /// </summary>
        public string CategoriesString => string.Join(", ", Categories);

    }
}
