using Bridge.Application.Places.Commands;
using Bridge.Application.Places.Queries;
using Bridge.Application.Places.ReadModels;
using Bridge.Domain.Places.Entities;
using Bridge.Shared;
using Bridge.Shared.Extensions;
using Bridge.WebApp.Services.Identity;

namespace Bridge.WebApp.Api.ApiClients.Admin
{
    public class AdminPlaceApiClient : JwtApiClient
    {
        public AdminPlaceApiClient(HttpClient httpClient, IAuthService authService) : base(httpClient, authService)
        {
        }

        /// <summary>
        /// 아이디로 장소 조회
        /// </summary>
        /// <param name="id">장소 아이디</param>
        /// <returns>장소</returns>
        public async Task<ApiResult<PlaceReadModel?>> GetPlaceById(long id)
        {
            return await SendAsync<PlaceReadModel?>(HttpMethod.Get, ApiRoutes.Admin.Places.Get.AddRouteParam("{id}", id));
        }

        /// <summary>
        /// 장소 목록을 조회한다.
        /// </summary>
        /// <param name="placeType">조회할 장소의 유형</param>
        /// <returns></returns>
        public async Task<ApiResult<List<PlaceReadModel>>> GetPlaceList(PlaceType placeType)
        {
            return await SendAsync<List<PlaceReadModel>>(HttpMethod.Get, ApiRoutes.Admin.Places.GetList
                .AddQueryParam("placeType", placeType));
        }

        /// <summary>
        /// 장소를 검색한다
        /// </summary>
        /// <param name="query">검색 쿼리</param>
        /// <returns></returns>
        public async Task<ApiResult<List<PlaceReadModel>>> SearchPlaces(SearchPlacesQuery query)
        {
            return await SendAsync<List<PlaceReadModel>>(HttpMethod.Post, ApiRoutes.Admin.Places.Search, query);
        }

        /// <summary>
        /// 장소를 생성한다
        /// </summary>
        /// <param name="command">장소</param>
        /// <returns></returns>
        public async Task<ApiResult<long>> CreatePlace(CreatePlaceCommand command)
        {
            return await SendAsync<long>(HttpMethod.Post, ApiRoutes.Admin.Places.Create, command);
        }

        /// <summary>
        /// 장소를 수정한다
        /// </summary>
        /// <param name="command">장소</param>
        /// <returns></returns>
        public async Task<ApiResult<Void>> UpdatePlace(UpdatePlaceCommand command)
        {
            return await SendAsync<Void>(HttpMethod.Put, ApiRoutes.Admin.Places.Update.AddRouteParam("id", command.Id), command);
        }

    }
}
