using Bridge.Application.Common;
using Bridge.Application.Places.ReadModels;
using Bridge.Domain.Places.Entities;

namespace Bridge.Application.Places.Repos
{
    public interface IPlaceReadRepository : IReadRepository<Place, PlaceReadModel>
    {
        /// <summary>
        /// 아이디로 장소 조회
        /// </summary>
        /// <param name="id">장소 아이디</param>
        /// <returns></returns>
        Task<PlaceReadModel?> GetPlaceAsync(long id);
    }
}
