using Bridge.Application.Places.Dtos;
using Bridge.Application.Places.ReadModels;
using Bridge.Domain.Places.Entities;

namespace Bridge.WebApp.Pages.Home.Models
{
    public class PlaceListModel
    {
        public static PlaceListModel ToPlaceModel(PlaceReadModel x)
        {
            return new PlaceListModel()
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
                OpeningTimes = x.OpeningTimes.Select(t => new OpeningTimeListModel()
                {
                    Day = t.Day,
                    Dayoff = t.Dayoff,
                    TwentyFourHours = t.TwentyFourHours,
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
        public long Id { get; set; }

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
        public List<PlaceCategory> Categories { get; set; } = new();

        /// <summary>
        /// 카테고리 문자열
        /// </summary>
        public string CategoriesString => string.Join(", ", Categories);

        /// <summary>
        /// 연락처
        /// </summary>
        public string? ContactNumber { get; set; }

        /// <summary>
        /// 영업시간
        /// </summary>
        public List<OpeningTimeListModel> OpeningTimes { get; set; } = new();

        /// <summary>
        /// 영업시간 보여주기 여부
        /// </summary>
        public bool ShowOpeningTimes { get; set; }
    }
}