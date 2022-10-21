using Bridge.Application.Places.Commands;
using Bridge.Application.Places.ReadModels;
using Bridge.Shared;
using Bridge.Shared.Extensions;
using Bridge.WebApp.Services.Identity;

namespace Bridge.WebApp.Api.ApiClients.Admin
{
    public class AdminRestroomApiClient : JwtApiClient
    {
        public AdminRestroomApiClient(HttpClient httpClient, IAuthService authService) : base(httpClient, authService)
        {
        }

        /// <summary>
        /// 아이디로 화장실 조회
        /// </summary>
        /// <param name="id">장소 아이디</param>
        /// <returns>장소</returns>
        public async Task<ApiResult<RestroomReadModel?>> GetRestroomById(long id)
        {
            return await SendAsync<RestroomReadModel?>(HttpMethod.Get, ApiRoutes.Admin.Restrooms.Get.AddRouteParam("{id}", id));
        }

        /// <summary>
        /// 화장실을 생성한다
        /// </summary>
        /// <param name="command">장소</param>
        /// <returns></returns>
        public async Task<ApiResult<long>> CreateRestroom(CreateRestroomCommand command)
        {
            return await SendAsync<long>(HttpMethod.Post, ApiRoutes.Admin.Restrooms.Create, command);
        }

        /// <summary>
        /// 화장실을 수정한다
        /// </summary>
        /// <param name="command">장소</param>
        /// <returns></returns>
        public async Task<ApiResult<Void>> UpdateRestroom(UpdateRestroomCommand command)
        {
            return await SendAsync<Void>(HttpMethod.Put, ApiRoutes.Admin.Restrooms.Update.AddRouteParam("id", command.Id), command);
        }
    }
}
