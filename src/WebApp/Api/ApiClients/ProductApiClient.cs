using Bridge.Application.Products.Commands;
using Bridge.Application.Products.ReadModels;
using Bridge.Shared;
using Bridge.Shared.Extensions;
using Bridge.WebApp.Services.Identity;

namespace Bridge.WebApp.Api.ApiClients
{
    public class ProductApiClient : JwtApiClient
    {
        public ProductApiClient(HttpClient httpClient, IAuthService authService) : base(httpClient, authService)
        {
        }
        
        /// <summary>
        /// 아이디로 제품 조회
        /// </summary>
        /// <param name="id">장소 아이디</param>
        /// <returns>장소</returns>
        public async Task<ApiResult<ProductReadModel?>> GetPlaceById(long id)
        {
            return await SendAsync<ProductReadModel?>(HttpMethod.Get, ApiRoutes.Products.Get.AddRouteParam("{id}", id));
        }

        /// <summary>
        /// 장소에 포함된 제품 목록을 조회한다.
        /// </summary>
        /// <param name="placeId">장소 아이디</param>
        /// <returns></returns>
        public async Task<ApiResult<List<ProductReadModel>>> GetPlaceList(long placeId)
        {
            return await SendAsync<List<ProductReadModel>>(HttpMethod.Get, ApiRoutes.Products.GetList
                .AddQueryParam("placeId", placeId));
        }

        /// <summary>
        /// 제품을 생성한다
        /// </summary>
        /// <param name="command">제품</param>
        /// <returns></returns>
        public async Task<ApiResult<long>> CreatePlace(CreateProductCommand command)
        {
            return await SendAsync<long>(HttpMethod.Post, ApiRoutes.Products.Create, command);
        }

        /// <summary>
        /// 제품을 수정한다
        /// </summary>
        /// <param name="command">제품</param>
        /// <returns></returns>
        public async Task<ApiResult<Void>> UpdatePlace(UpdateProductCommand command)
        {
            return await SendAsync<Void>(HttpMethod.Put, ApiRoutes.Products.Update.AddRouteParam("id", command.ProductId), command);
        }

    }
}
