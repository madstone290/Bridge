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

        private readonly List<OpeningTimeFormModel> _openingTimes = new()
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
        /// 속성 복사를 한다
        /// </summary>
        /// <param name="source">원본 객체</param>
        /// <param name="target">대상 객체</param>
        public static void Copy(PlaceFormModel source, PlaceFormModel target)
        {
            target.Id = source.Id;
            target.Type = source.Type;
            target.Name = source.Name;
            target.BaseAddress = source.BaseAddress;
            target.DetailAddress = source.DetailAddress;
            target.ImageChanged = source.ImageChanged;
            target.ImageName = source.ImageName;
            target.ImageData = source.ImageData;
            target.ImageUrl = source.ImageUrl;
            target.Categories = source.Categories;
            target.ContactNumber = source.ContactNumber;
            target.OpeningTimes = source.OpeningTimes.Select(x => new OpeningTimeFormModel(x.Day)
            {
                Day = x.Day,
                Dayoff = x.Dayoff,
                OpenTime = x.OpenTime,
                CloseTime = x.CloseTime,
                TwentyFourHours = x.TwentyFourHours,
                BreakStartTime = x.BreakStartTime,
                BreakEndTime = x.BreakEndTime,
            });
        }

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
        /// 이미지 변경 여부
        /// </summary>
        public bool ImageChanged { get; set; }

        /// <summary>
        /// 이미지 이름
        /// </summary>
        public string? ImageName { get; set; }

        /// <summary>
        /// 이미지 데이터
        /// </summary>
        public byte[]? ImageData { get; set; }

        /// <summary>
        /// 이미지 Url
        /// </summary>
        public string? ImageUrl { get; set; }


        /// <summary>
        /// 영업시간
        /// </summary>
        public IEnumerable<OpeningTimeFormModel> OpeningTimes
        {
            get => _openingTimes;
            set
            {
                foreach(var openingTime in _openingTimes.ToArray())
                {
                    var entry = value.FirstOrDefault(x => x.Day == openingTime.Day);
                    if(entry!= null)
                    {
                        _openingTimes.Remove(_openingTimes.First(x => x.Day == openingTime.Day));
                        _openingTimes.Add(entry);
                    }
                }
            }
        }

        /// <summary>
        /// 영업시간(월요일부터)
        /// </summary>
        public IEnumerable<OpeningTimeFormModel> OpeningTimesFromMonday
        {
            get
            {
                yield return _openingTimes.First(x => x.Day == DayOfWeek.Monday);
                yield return _openingTimes.First(x => x.Day == DayOfWeek.Tuesday);
                yield return _openingTimes.First(x => x.Day == DayOfWeek.Wednesday);
                yield return _openingTimes.First(x => x.Day == DayOfWeek.Thursday);
                yield return _openingTimes.First(x => x.Day == DayOfWeek.Friday);
                yield return _openingTimes.First(x => x.Day == DayOfWeek.Saturday);
                yield return _openingTimes.First(x => x.Day == DayOfWeek.Sunday);
            }
        }
    }
}
