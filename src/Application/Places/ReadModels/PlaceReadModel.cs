using Bridge.Application.Places.Dtos;
using Bridge.Domain.Places.Enums;

namespace Bridge.Application.Places.ReadModels
{
    public class PlaceReadModel
    {
        #region Stored Properties

        /// <summary>
        /// 소유자
        /// </summary>
        public string OwnerId { get; set; } = string.Empty;

        /// <summary>
        /// 아이디
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 장소유형
        /// </summary>
        public PlaceType Type { get; set; }

        /// <summary>
        /// 상태
        /// </summary>
        public PlaceStatus Status { get; set; }

        /// <summary>
        /// 생성일시
        /// </summary>
        public DateTime CreationDateTimeUtc { get; set; }

        /// <summary>
        /// 최근 업데이트 일시
        /// </summary>
        public DateTime? LastUpdateDateTimeUtc { get; set; }

        /// <summary>
        /// 생성일시
        /// </summary>
        public DateTime CreationDateTimeLocal => CreationDateTimeUtc.ToLocalTime();

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
        /// 이미지 경로
        /// </summary>
        public string? ImagePath { get; set; }

        /// <summary>
        /// 영업시간
        /// </summary>
        public List<OpeningTimeDto> OpeningTimes { get; set; } = new();

        #endregion

        #region Computed Properties

        /// <summary>
        /// 검색 지점에서 장소까지의 거리
        /// </summary>
        public double? Distance { get; set; }

        #endregion


    }
}
