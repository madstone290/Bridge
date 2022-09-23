using Bridge.Application.Places.ReadModels;
using Bridge.Shared;
using Bridge.Shared.Extensions;

namespace Bridge.WebApp.Api.ApiClients
{
    public class PlaceApiClient : ApiClient
    {
        public PlaceApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

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
    }
}
