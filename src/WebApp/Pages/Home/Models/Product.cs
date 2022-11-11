using Bridge.Domain.Products.Enums;

namespace Bridge.WebApp.Pages.Home.Models
{
    public class Product
    {
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
        /// 제품 이미지 소스
        /// </summary>
        public string? ImageSrc { get; set; }

        /// <summary>
        /// 제품유형 문자열
        /// </summary>
        public string TypeString => Type.ToString();

        /// <summary>
        /// 제품 가격 문자열
        /// </summary>
        public string PriceString => Price?.ToString() ?? string.Empty;

        /// <summary>
        /// 카테고리 문자열
        /// </summary>
        public string CategoriesString => string.Join(", ", Categories);


        /// <summary>
        /// 거리
        /// </summary>
        public double? Distance { get; set; }

        /// <summary>
        /// 거리
        /// </summary>
        public string DistanceString
        {
            get
            {
                if (Distance < 1000)
                    return $"{Distance:0}m";
                else
                    return $"{Distance / 1000:0.0}km";
            }
        }
    }
}
