using Bridge.Application.Places.Commands;
using Bridge.Application.Places.Queries;
using Bridge.Application.Places.ReadModels;
using Bridge.Shared;

namespace Bridge.WebApp.Api.ApiClients
{
    public class PlaceApiClient : ApiClient
    {
        public PlaceApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<ApiResult<Guid>> AddPlace(CreatePlaceCommand command)
        {
            return await SendAsync<Guid>(HttpMethod.Post, ApiRoutes.Places.Create, command);
        }

        /// <summary>
        /// 장소를 검색한다
        /// </summary>
        /// <param name="query">검색 쿼리</param>
        /// <returns></returns>
        public async Task<ApiResult<List<PlaceReadModel>>> SearchPlaces(SearchPlacesQuery query)
        {
            return await SendAsync<List<PlaceReadModel>>(HttpMethod.Post, ApiRoutes.Places.Search, query);
        }

    }
}
