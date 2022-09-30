namespace Bridge.Infrastructure.NaverMaps.Data
{
    /// <summary>
    /// 응답 메타데이터
    /// </summary>
    public class Meta
    {
        public static Meta Empty { get; } = new();

        /// <summary>
        /// 전체 결과 개수
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 현재 페이지 번호
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 페이지 내 결과 개수
        /// </summary>
        public int Count { get; set; }
    }

}
