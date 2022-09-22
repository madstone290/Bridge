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

        /// <summary>
        /// 좌우 동향, 상하 북향 사이에 존재하는 장소 조회
        /// </summary>
        /// <param name="leftEasting">좌 동향</param>
        /// <param name="rightEasting">우 동향</param>
        /// <param name="bottomNorthing">하 북향</param>
        /// <param name="topNorthing">북 북향</param>
        /// <returns>장소 목록</returns>
        Task<List<PlaceReadModel>> GetPlacesByEastingBetweenAndNorthingBetween(double leftEasting, double rightEasting, double bottomNorthing, double topNorthing);
    }
}
