using Bridge.Application.Common;
using Bridge.Application.Places.ReadModels;
using Bridge.Domain.Places.Entities;

namespace Bridge.Application.Places.Repos
{
    public interface IPlaceReadRepository : IReadRepository<Place, PlaceReadModel>
    {
        /// <summary>
        /// 주어진 위치에서 검색어로 장소를 검색한다.
        /// </summary>
        /// <param name="searchText">검색어</param>
        /// <param name="easting">검색위치 동향</param>
        /// <param name="northing">검색위치 북향</param>
        /// <param name="maxCount">장소 검색 수</param>
        /// <param name="maxDistance">최대 검색거리</param>
        /// <returns></returns>
        Task<List<PlaceReadModel>> SearchPlacesAsync(string searchText, double easting, double northing, int maxCount = 1000, int? maxDistance = null);

        /// <summary>
        /// 페이지
        /// </summary>
        /// <param name="placeType"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PaginatedList<PlaceReadModel>> GetPaginatedPlacesAsync(PlaceType? placeType = null, int pageNumber = 1, int pageSize = 100);
    }
}
