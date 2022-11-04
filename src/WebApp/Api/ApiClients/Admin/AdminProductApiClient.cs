using Bridge.Application.Common;
using Bridge.Application.Products.Commands;
using Bridge.Application.Products.ReadModels;
using Bridge.Shared;
using Bridge.Shared.Extensions;
using Bridge.WebApp.Services.Identity;

namespace Bridge.WebApp.Api.ApiClients.Admin
{
    public class AdminProductApiClient : JwtApiClient
    {
        public AdminProductApiClient(HttpClient httpClient, IAuthService authService) : base(httpClient, authService)
        {
        }

        /// <summary>
        /// 아이디로 제품 조회
        /// </summary>
        /// <param name="id">장소 아이디</param>
        /// <returns>장소</returns>
        public async Task<ApiResult<ProductReadModel?>> GetProductById(Guid id)
        {
            return await SendAsync<ProductReadModel?>(HttpMethod.Get, ApiRoutes.Admin.Products.Get.AddRouteParam("{id}", id));
        }

        /// <summary>
        /// 장소에 포함된 제품 목록을 조회한다.
        /// </summary>
        /// <param name="placeId">장소 아이디</param>
        /// <returns></returns>
        public async Task<ApiResult<List<ProductReadModel>>> GetProductList(Guid placeId)
        {
            return await SendAsync<List<ProductReadModel>>(HttpMethod.Get, ApiRoutes.Admin.Products.GetList
                .AddQueryParam("placeId", placeId));
        }

        /// <summary>
        /// 장소에 포함된 제품 목록을 조회한다.
        /// </summary>
        /// <param name="placeId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<ApiResult<PaginatedList<ProductReadModel>>> GetPaginatedProductList(Guid placeId, int pageNumber, int pageSize)
        {
            return await SendAsync<PaginatedList<ProductReadModel>>(HttpMethod.Get, ApiRoutes.Admin.Products.GetPaginatedList
                .AddQueryParam("placeId", placeId)
                .AddQueryParam("pageNumber", pageNumber)
                .AddQueryParam("pageSize", pageSize));
        }

        /// <summary>
        /// 제품을 생성한다
        /// </summary>
        /// <param name="command">제품</param>
        /// <returns></returns>
        public async Task<ApiResult<Guid>> CreateProduct(CreateProductCommand command)
        {
            return await SendAsync<Guid>(HttpMethod.Post, ApiRoutes.Admin.Products.Create, command);
        }

        /// <summary>
        /// 제품을 수정한다
        /// </summary>
        /// <param name="command">제품</param>
        /// <returns></returns>
        public async Task<ApiResult<Void>> UpdateProduct(UpdateProductCommand command)
        {
            return await SendAsync<Void>(HttpMethod.Put, ApiRoutes.Admin.Products.Update.AddRouteParam("id", command.Id), command);
        }


        /// <summary>
        /// 제품을 폐기한다
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<ApiResult<Void>> DiscardProduct(Guid id)
        {
            return await SendAsync<Void>(HttpMethod.Put, ApiRoutes.Admin.Products.Discard.AddRouteParam("id", id));
        }

    }
}
