using Bridge.Application.Places.ReadModels;
using Bridge.Domain.Places.Enums;
using FluentValidation;

namespace Bridge.WebApp.Pages.Common.Models
{
    public class Place
    {
        public class Validator : BaseValidator<Place>
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

        private readonly List<OpeningTime> _openingTimes = new()
        {
            new OpeningTime(DayOfWeek.Monday),
            new OpeningTime(DayOfWeek.Tuesday),
            new OpeningTime(DayOfWeek.Wednesday),
            new OpeningTime(DayOfWeek.Thursday),
            new OpeningTime(DayOfWeek.Friday),
            new OpeningTime(DayOfWeek.Saturday),
            new OpeningTime(DayOfWeek.Sunday),
        };


        public static Place Create(PlaceReadModel x)
        {
            return new Place()
            {
                Id = x.Id,
                Type = x.Type,
                Name = x.Name,
                BaseAddress = x.Address.BaseAddress,
                DetailAddress = x.Address.DetailAddress,
                Latitude = x.Location.Latitude,
                Longitude = x.Location.Longitude,
                Distance = x.Distance,
                Easting = x.Location.Easting,
                Northing = x.Location.Northing,
                Categories = x.Categories.ToList(),
                ContactNumber = x.ContactNumber,
                OpeningTimes = x.OpeningTimes.Select(t => new OpeningTime()
                {
                    Day = t.Day,
                    IsDayoff = t.IsDayoff,
                    Is24Hours = t.Is24Hours,
                    OpenTime = t.OpenTime,
                    CloseTime = t.CloseTime,
                    BreakStartTime = t.BreakStartTime,
                    BreakEndTime = t.BreakEndTime
                }).ToList()
            };
        }

        /// <summary>
        /// 아이디
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 장소유형
        /// </summary>
        public PlaceType Type { get; set; }

        /// <summary>
        /// 장소유형
        /// </summary>
        public string TypeString => Type.ToString();

        /// <summary>
        /// 장소명
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 주소
        /// </summary>
        public string Address => BaseAddress + " " + DetailAddress;

        /// <summary>
        /// 기본주소
        /// </summary>
        public string BaseAddress { get; set; } = string.Empty;

        /// <summary>
        /// 상세주소
        /// </summary>
        public string DetailAddress { get; set; } = string.Empty;

        /// <summary>
        /// 거리
        /// </summary>
        public double? Distance { get; set; }

        /// <summary>
        /// 거리
        /// </summary>
        public string DistanceString => $"{Distance:0}m";

        /// <summary>
        /// 위도
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 경도
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 위경도문자열
        /// </summary>
        public string LatitudeLongitudeString => $"{Latitude:0.000000},{Longitude:0.000000}";

        /// <summary>
        /// UTM-K도법의 동쪽방향 좌표
        /// </summary>
        public double Easting { get; set; }

        /// <summary>
        /// UTM-K도법의 북쪽방향 좌표
        /// </summary>
        public double Northing { get; set; }

        /// <summary>
        /// 동북향 문자열
        /// </summary>
        public string EastingNorthingString => $"{Easting:0},{Northing:0}";

        /// <summary>
        /// 장소 카테고리
        /// </summary>
        public IEnumerable<PlaceCategory> Categories { get; set; } = Enumerable.Empty<PlaceCategory>();

        /// <summary>
        /// 카테고리 문자열
        /// </summary>
        public string CategoriesString => string.Join(", ", Categories);

        /// <summary>
        /// 연락처
        /// </summary>
        public string? ContactNumber { get; set; }

        /// <summary>
        /// 영업시간 보여주기 여부
        /// </summary>
        public bool ShowOpeningTimes { get; set; }

        #region Form 속성
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
        #endregion

        /// <summary>
        /// 영업시간
        /// </summary>
        public IEnumerable<OpeningTime> OpeningTimes
        {
            get => _openingTimes;
            set
            {
                foreach (var openingTime in _openingTimes.ToArray())
                {
                    var entry = value.FirstOrDefault(x => x.Day == openingTime.Day);
                    if (entry != null)
                    {
                        _openingTimes.Remove(_openingTimes.First(x => x.Day == openingTime.Day));
                        _openingTimes.Add(entry);
                    }
                }
            }
        }

        public IEnumerable<OpeningTime> OpeningTimesFromMonday
        {
            get
            {
                var openingTimes = new List<OpeningTime>
                {
                    OpeningTimes.First(x => x.Day == DayOfWeek.Monday),
                    OpeningTimes.First(x => x.Day == DayOfWeek.Tuesday),
                    OpeningTimes.First(x => x.Day == DayOfWeek.Wednesday),
                    OpeningTimes.First(x => x.Day == DayOfWeek.Thursday),
                    OpeningTimes.First(x => x.Day == DayOfWeek.Friday),
                    OpeningTimes.First(x => x.Day == DayOfWeek.Saturday),
                    OpeningTimes.First(x => x.Day == DayOfWeek.Sunday)
                };
                return openingTimes;
            }
        }

    }
}
