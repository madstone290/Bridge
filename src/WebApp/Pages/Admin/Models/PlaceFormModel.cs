using Bridge.Domain.Places.Entities;
using FluentValidation;

namespace Bridge.WebApp.Pages.Admin.Models
{
    public class PlaceFormModel
    {
        public class Validator : BaseValidator<PlaceFormModel>
        {
            public Validator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage("* 필수");

                RuleFor(x => x.BaseAddress)
                    .NotEmpty()
                    .WithMessage("* 필수");

            }
        }

        private List<OpeningTimeFormModel> _openingTimes = new()
        {
            new OpeningTimeFormModel(DayOfWeek.Monday),
            new OpeningTimeFormModel(DayOfWeek.Tuesday),
            new OpeningTimeFormModel(DayOfWeek.Wednesday),
            new OpeningTimeFormModel(DayOfWeek.Thursday),
            new OpeningTimeFormModel(DayOfWeek.Friday),
            new OpeningTimeFormModel(DayOfWeek.Saturday),
            new OpeningTimeFormModel(DayOfWeek.Sunday),
        };

        private List<PlaceCategory> _categories = new();

        /// <summary>
        /// 아이디
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 장소유형
        /// </summary>
        public PlaceType Type { get; set; }

        /// <summary>
        /// 장소명
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 기본주소
        /// </summary>
        public string BaseAddress { get; set; } = string.Empty;

        /// <summary>
        /// 상세주소
        /// </summary>
        public string DetailAddress { get; set; } = string.Empty;

        /// <summary>
        /// 장소 카테고리
        /// </summary>
        public IEnumerable<PlaceCategory> Categories
        {
            get => _categories;
            set => _categories = value.ToList();
        }

        /// <summary>
        /// 연락처
        /// </summary>
        public string? ContactNumber { get; set; }

        /// <summary>
        /// 영업시간
        /// </summary>
        public IEnumerable<OpeningTimeFormModel> OpeningTimes
        { 
            get => _openingTimes;
            set => _openingTimes = value.ToList();
        }

    }
}
