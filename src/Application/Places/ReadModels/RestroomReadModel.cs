using Bridge.Application.Places.Dtos;
using Bridge.Domain.Places.Enums;

namespace Bridge.Application.Places.ReadModels
{
    public class RestroomReadModel 
    {
        #region Stored Properties

        /// <summary>
        /// 아이디
        /// </summary>
        public long Id { get; set; }

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
        /// 영업시간
        /// </summary>
        public List<OpeningTimeDto> OpeningTimes { get; set; } = new();

        /// <summary>
        /// 남녀공용여부
        /// </summary>
        public bool IsUnisex { get; set; }

        /// <summary>
        /// 기저귀 교환대 위치
        /// </summary>
        public DiaperTableLocation? DiaperTableLocation { get; set; }

        public int? MaleToilet { get; set; }

        public int? MaleUrinal { get; set; }

        public int? MaleDisabledToilet { get; set; }

        public int? MaleDisabledUrinal { get; set; }

        public int? MaleKidToilet { get; set; }

        public int? MaleKidUrinal { get; set; }

        public int? FemaleToilet { get; set; }

        public int? FemaleKidToilet { get; set; }

        public int? FemaleDisabledToilet { get; set; }


        #endregion

        #region Computed Properties

        /// <summary>
        /// 검색 지점에서 장소까지의 거리
        /// </summary>
        public double? Distance { get; set; }

        #endregion
    }
}
