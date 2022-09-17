using Bridge.Application.Places.ReadModels;

namespace Bridge.Application.Places.Repos
{
    public interface IPlaceReadRepository
    {
        /// <summary>
        /// 아이디로 장소 조회
        /// </summary>
        /// <param name="id">장소 아이디</param>
        /// <returns></returns>
        Task<PlaceReadModel?> GetPlaceAsync(long id);
     
    }
}
