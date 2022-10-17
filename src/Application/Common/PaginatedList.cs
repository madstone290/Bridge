namespace Bridge.Application.Common
{
    public class PaginatedList<T>
    {
        public PaginatedList(List<T> list, int totalCount, int pageNumber, int pageSize)
        {
            List = list;
            PageNumber = pageNumber;
            TotalCount = totalCount;
            PageSize = pageSize;
        }

        /// <summary>
        /// 현재 페이지 데이터
        /// </summary>
        public List<T> List { get; init; }

        /// <summary>
        /// 전체 데이터 수
        /// </summary>
        public int TotalCount { get; init; }

        /// <summary>
        /// 현재 페이지 번호
        /// </summary>
        public int PageNumber { get; init; }

        /// <summary>
        /// 페이지 크기
        /// </summary>
        public int PageSize{ get; init; }

        /// <summary>
        /// 전체 페이지 수
        /// </summary>
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;
    }
}
