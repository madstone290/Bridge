using Bridge.Application.Products.Queries;
using Bridge.Application.Products.ReadModels;
using Bridge.Shared;
using Bridge.Shared.Extensions;

namespace Bridge.WebApp.Api.ApiClients
{
    public class ProductApiClient : ApiClient
    {
        public ProductApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        /// <summary>
        /// 제품을 검색한다
        /// </summary>
        /// <param name="query">검색 쿼리</param>
        /// <returns></returns>
        public async Task<ApiResult<List<ProductReadModel>>> SearchAsync(SearchProductsQuery query)
        {
            return await SendAsync<List<ProductReadModel>>(HttpMethod.Post, ApiRoutes.Products.Search, query);
        }

        /// <summary>
        /// 장소에서 판매중인 제품을 조회한다
        /// </summary>
        /// <param name="placeId"></param>
        /// <returns></returns>
        public async Task<ApiResult<List<ProductReadModel>>> GetProductList(Guid placeId)
        {
            return await SendAsync<List<ProductReadModel>>(HttpMethod.Get, ApiRoutes.Products.GetList.AddQueryParam("placeId", placeId));
        }

    }
}
