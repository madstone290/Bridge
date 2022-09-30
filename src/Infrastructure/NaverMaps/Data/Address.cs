namespace Bridge.Infrastructure.NaverMaps.Data
{
    /// <summary>
    /// 주소 정보
    /// </summary>
    public class Address
    {
        /// <summary>
        /// 도로명 주소
        /// </summary>
        public string RoadAddress { get; set; } = string.Empty;

        /// <summary>
        /// 지번주소
        /// </summary>
        public string JibunAddress { get; set; } = string.Empty;

        /// <summary>
        /// 주소 구성 요소
        /// </summary>
        public IEnumerable<Element> AddressElements { get; set; } = Enumerable.Empty<Element>();

        /// <summary>
        /// X좌표(경도)
        /// </summary>
        public string X { get; set; } = string.Empty;

        /// <summary>
        /// Y좌표(위도)
        /// </summary>
        public string Y { get; set; } = string.Empty;

        /// <summary>
        /// 시/도
        /// </summary>
        public string SIDO => AddressElements.First(x => x.Type == ElementType.SIDO).LongName;

        /// <summary>
        /// 시/구/군
        /// </summary>
        public string SIGUGUN => AddressElements.First(x => x.Type == ElementType.SIGUGUN).LongName;

        /// <summary>
        /// 읍면동
        /// </summary>
        public string DONGMYUN => AddressElements.First(x => x.Type == ElementType.DONGMYUN).LongName;

        /// <summary>
        /// 리
        /// </summary>
        public string RI => AddressElements.First(x => x.Type == ElementType.RI).LongName;

        /// <summary>
        /// 지번
        /// </summary>
        public string LAND_NUMBER => AddressElements.First(x => x.Type == ElementType.LAND_NUMBER).LongName;

        /// <summary>
        /// 도로명
        /// </summary>
        public string ROAD_NAME => AddressElements.First(x => x.Type == ElementType.ROAD_NAME).LongName;

        /// <summary>
        /// 건물번호
        /// </summary>
        public string BUILDING_NUMBER => AddressElements.First(x => x.Type == ElementType.BUILDING_NUMBER).LongName;

        /// <summary>
        /// 건물명
        /// </summary>
        public string BUILDING_NAME => AddressElements.First(x => x.Type == ElementType.BUILDING_NAME).LongName;

        /// <summary>
        /// 우편번호
        /// </summary>
        public string POSTAL_CODE => AddressElements.First(x => x.Type == ElementType.POSTAL_CODE).LongName;


    }

}
