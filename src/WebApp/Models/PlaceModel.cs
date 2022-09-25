using Bridge.Application.Places.Dtos;
using Bridge.Application.Places.ReadModels;
using Bridge.Domain.Places.Entities;

namespace Bridge.WebApp.Models
{
    public class PlaceModel
    {
        public static PlaceModel ToPlaceModel(PlaceReadModel x)
        {
            return new PlaceModel()
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                Latitude = x.Location.Latitude,
                Longitude = x.Location.Longitude,
                Easting = x.Location.Easting,
                Northing = x.Location.Northing,
                Categories = x.Categories,
                ContactNumber = x.ContactNumber,
                OpeningTimes = x.OpeningTimes
            };
        }

        /// <summary>
        /// 아이디
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 장소명
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 주소
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 거리
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// 거리
        /// </summary>
        public string DistanceString { get; set; } = string.Empty;

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
        public string LatitudeLongitudeString => $"{Latitude:0.0000},{Latitude:0.0000}";

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
        public List<OpeningTimeDto> OpeningTimes { get; set; } = new();

        /// <summary>
        /// 영업시간 보여주기 여부
        /// </summary>
        public bool ShowOpeningTimes { get; set; }

        /// <summary>
        /// 주어진 UTM-K 좌표에서의 거리를 계산한다.
        /// </summary>
        /// <param name="easting"></param>
        /// <param name="northing"></param>
        public void CalcDistance(double easting, double northing)
        {
            Distance = Math.Sqrt(Math.Pow(Math.Abs(Easting - easting), 2) + Math.Pow(Math.Abs(Northing - northing), 2));
            DistanceString = string.Format("{0:0}m", Distance);
        }
    }
}
