namespace Bridge.Infrastructure.NaverMaps.Data
{

    /// <summary>
    /// 주소 구성 요소
    /// </summary>
    public class Element
    {
        /// <summary>
        /// 요소 타입. 컬렉션 응답이나 실제로 하나의 데이터만 있다.
        /// </summary>
        public IEnumerable<string> Types { get; set; } = Enumerable.Empty<string>();

        /// <summary>
        /// 요소 타입
        /// </summary>
        public string Type => Types.FirstOrDefault() ?? string.Empty;

        /// <summary>
        /// 이름
        /// </summary>
        public string LongName { get; set; } = string.Empty;
    }
}
