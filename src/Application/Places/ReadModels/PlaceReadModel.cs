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
        /// 장소유형
        /// </summary>
        public PlaceType Type { get; set; }

        /// <summary>
        /// 장소명
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 주소
        /// </summary>
        public AddressDto Address { get; set; } = AddressDto.Empty;

        /// <summary>
        /// 위치
        /// </summary>
        public PlaceLocationDto Location { get; set; } = new();

        /// <summary>
        /// 장소 카테고리
        /// </summary>
        public IEnumerable<PlaceCategory> Categories { get; set; } = Enumerable.Empty<PlaceCategory>();

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
