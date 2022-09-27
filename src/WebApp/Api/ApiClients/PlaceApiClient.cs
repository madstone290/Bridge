using Bridge.Api.Controllers.Dtos;
using Bridge.Application.Places.Commands;
using Bridge.Application.Places.ReadModels;
using Bridge.Domain.Places.Entities;
using Bridge.Shared;
using Bridge.Shared.Extensions;

namespace Bridge.WebApp.Api.ApiClients
{
    public class PlaceApiClient : ApiClient
    {
        public PlaceApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        /// <summary>
        /// 아이디로 장소 조회
        /// </summary>
        /// <param name="id">장소 아이디</param>
        /// <returns>장소</returns>
        public async Task<ApiResult<PlaceReadModel?>> GetPlaceById(long id)
        {
            return await SendAsync<PlaceReadModel?>(HttpMethod.Get, ApiRoutes.Places.Get.AddRouteParam("{id}", id));
        }

        /// <summary>
        /// 이름 및 지역으로 장소목록 조회
        /// </summary>
        /// <param name="name">이름. 이름을 포함하는 모든 장소를 조회한다.</param>
        /// <param name="leftEasting"></param>
        /// <param name="rightEasting"></param>
        /// <param name="bottomNorthing"></param>
        /// <param name="topNorthing"></param>
        /// <returns></returns>
        public async Task<ApiResult<List<PlaceReadModel>>> GetPlacesByNameAndRegion(string name,
                                                                         double leftEasting,
                                                                         double rightEasting,
                                                                         double bottomNorthing,
                                                                         double topNorthing)
        {
            return await SendAsync<List<PlaceReadModel>>(HttpMethod.Get, ApiRoutes.Places.GetList
                .AddQueryParam("name", name)
                .AddQueryParam("leftEasting", leftEasting)
                .AddQueryParam("rightEasting", rightEasting)
                .AddQueryParam("bottomNorthing", bottomNorthing)
                .AddQueryParam("topNorthing", topNorthing));
        }

        /// <summary>
        /// 주어진 장소유형에 해당하는 모든 장소를 조회한다.
        /// </summary>
        /// <param name="placeType">장소유형</param>
        /// <returns>장소 목록</returns>
        public async Task<ApiResult<List<PlaceReadModel>>> GetPlacesByPlaceType(PlaceType placeType)
        {
            return await SendAsync<List<PlaceReadModel>>(HttpMethod.Post, ApiRoutes.Places.Search, new PlaceSearchDto() { PlaceType = placeType });
        }

        /// <summary>
        /// 장소를 생성한다
        /// </summary>
        /// <param name="command">장소</param>
        /// <returns></returns>
        public async Task<ApiResult<long>> CreatePlace(CreatePlaceCommand command)
        {
            return await SendAsync<long>(HttpMethod.Post, ApiRoutes.Places.Create, command);
        }


    }
}
