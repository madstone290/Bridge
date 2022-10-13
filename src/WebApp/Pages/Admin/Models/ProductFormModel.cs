using Bridge.Domain.Products.Entities;
using FluentValidation;

namespace Bridge.WebApp.Pages.Admin.Models
{
    public class ProductFormModel
    {
        public class Validator : BaseValidator<ProductFormModel>
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
    }
}
