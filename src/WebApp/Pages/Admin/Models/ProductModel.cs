using Bridge.Application.Products.ReadModels;
using Bridge.Domain.Products.Entities;
using FluentValidation;

namespace Bridge.WebApp.Pages.Admin.Models
{
    public class ProductModel
    {
        public class Validator : BaseValidator<ProductModel>
        {
            public Validator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage("* 필수");

                RuleFor(x => x.PlaceId)
                    .GreaterThan(0)
                    .WithMessage("* 필수");
            }
        }

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

        public long Id { get; set; }

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
        public long PlaceId { get; set; }

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
        public string PriceString => Price?.ToString() ?? string.Empty;

        /// <summary>
        /// 카테고리 문자열
        /// </summary>
        public string CategoriesString => string.Join(", ", Categories);
    }
}
