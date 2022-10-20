using Bridge.Application.Places.Dtos;
using Bridge.Application.Places.ReadModels;
using Bridge.Domain.Places.Entities;

namespace Bridge.WebApp.Pages.Home.Models
{
    public class PlaceListModel
    {
        public const string NoPictureUrl = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAMTSURBVEhL1ZbNK/RRFMe/M14aKcrKS5MRCysWyhaN1NiItSkmLKxs/BNKoRRCpsZrBksRSdhgSknyuvOS18JMXs9zz3Hn/afnyfNYPJ+a7v2de2bOued87/2NiRT4Qcx6/DH+SYDX11d8VYhvl8jv92N6ehqPj49ISEiQICkpKaisrERubq720gE+Pj5weHgIHv+E5+dnLC0tobW1VTI/OTlBWloarFYrxsfHUVBQgNLSUvGVEg0ODuL6+hpPT0+//XDGHo8HbW1tkhCPZrMZR0dH6OnpQV1dHXZ3d/Hw8CABOANSCzyEeH9/17N4jo+PaWVlhXZ2dmh1dZW2trZobm5O1lTZqK+vj9SP08jIiNiimszZdXV1YWhoCAMDA4Yl29zcRElJiYzsz+Xi0szOzkoPkpOTkZqaKnYmKsDMzIzUtbm5GVVVVVheXtYrYdTupKk8OhwO3N/f4+XlBTabTYKopLXnJ1EBeJHrySQlJckXYykqKsLe3h6ysrJwcXGB6upq3NzciIo4SCAQkHkIrlOwB8qROjs7aWxsjLq7u+nt7U3skaiykSqjrLEP94Br39HRQQcHB+Lj9Xrp/Pxc5oZNjvzhy8tLPQujJE39/f0SbHt7m1TPaH19XZ7X1taovb1de8Y0OQjXmJmYmBBdswQjYZ1zaZRiRJKFhYVQicgzk5OTI6PAUWJ3wCiV0OLiYqgk6gzolTAs1bu7Ozo7OyPVbG0l8Q9iuIPb21v4fD459iaTCS0tLaHsgszPz+Pq6grDw8PIzMxEenq6XokmLgBrn8+By+XSFoi+a2pqMDk5Kc8LCwuwWCyw2+2ora2FEoXYjYgLwNeA0+lEYmKitnySn58v9w1fB0oEKC8vF3teXh6ys7OxsbEhz7FEBVBKkJuQt2wEH6yGhgZpcCQVFRVQVwhUL7QlTCgAH5r9/X2UlZVpizG8CyPq6+sxNTUVdzglAB/70dFRNDY2ivE7sBiamprQ29urLZ/I+8DtdsudkpGRoc3f5/T0FMXFxSIKJvRG48bp6V/BO4kUyP/+rwL4BcbojOt/lg/uAAAAAElFTkSuQmCC";

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

        /// <summary>
        /// 이미지 URL
        /// </summary>
        public string? ImageUrl { get; set; }

    }
}
