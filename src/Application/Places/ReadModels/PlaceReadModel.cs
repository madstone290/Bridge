using Bridge.Application.Places.Dtos;
using Bridge.Domain.Places.Entities;

namespace Bridge.Application.Places.ReadModels
{
    public class PlaceReadModel
    {
        /// <summary>
        /// 아이디
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 장소명
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 장소 위도
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 장소 경도
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 장소 카테고리
        /// </summary>
        public List<PlaceCategory> Categories { get; set; } = new();

        /// <summary>
        /// 연락처
        /// </summary>
        public string? ContactNumber { get; set; }

        /// <summary>
        /// 영업시간
        /// </summary>
        public List<OpeningTimeDto> OpeningTimes { get; set; } = new();
    }
}
