using Bridge.Application.Products.ReadModels;
using Bridge.Domain.Products.Enums;

namespace Bridge.WebApp.Pages.Home.Models
{
    public class Product
    {
        public static Product Create(ProductReadModel x)
        {
            return new Product()
            {
                Id = x.Id,
                Place = x.Place == null ? null : Place.Create(x.Place),
                Type = x.Type,
                Name = x.Name,
                PlaceId = x.PlaceId,
                Price = x.Price,
                Categories= x.Categories,
            };
        }

        public Guid Id { get; set; }

        public Place? Place { get; set; }

        /// <summary>
        /// 제품타입
        /// </summary>
        public ProductType Type { get; set; }

        /// <summary>
        /// 제품명
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 제품이 판매되는 장소
        /// </summary>
        public Guid PlaceId { get; set; }

        /// <summary>
        /// 제품 가격
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// 제품 범주
        /// </summary>
        public IEnumerable<ProductCategory> Categories { get; set; } = Enumerable.Empty<ProductCategory>();

        /// <summary>
        /// 제품유형 문자열
        /// </summary>
        public string TypeString => Type.ToString();

        /// <summary>
        /// 제품 가격 문자열
        /// </summary>
        public string PriceString => $"{Price:0}원" ?? string.Empty;

        /// <summary>
        /// 카테고리 문자열
        /// </summary>
        public string CategoriesString => string.Join(", ", Categories);

        /// <summary>
        /// 제품 이미지
        /// </summary>
        public string? ImageSrc { get; set; }


    }
}
