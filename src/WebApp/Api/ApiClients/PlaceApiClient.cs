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
