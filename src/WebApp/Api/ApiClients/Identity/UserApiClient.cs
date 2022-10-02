using Bridge.Api.Controllers.Identity.Dtos;
using Bridge.Infrastructure.Identity;
using Bridge.Shared;

namespace Bridge.WebApp.Api.ApiClients.Identity
{
    public class UserApiClient : ApiClient
    {
        public UserApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<ApiResult<long>> RegisterAsync(UserDto userDto)
        {
            return await SendAsync<long>(HttpMethod.Post, ApiRoutes.Users.Register, userDto);
        }

        public async Task<ApiResult<Void>> SendVerificationEmailAsync(VerificationDto verificationDto)
        {
            return await SendAsync<Void>(HttpMethod.Post, ApiRoutes.Users.SendVerificationEmail, verificationDto);
        }

        public async Task<ApiResult<TokenResult>> LoginAsync(LoginDto loginDto)
        {
            return await SendAsync<TokenResult>(HttpMethod.Post, ApiRoutes.Users.Login, loginDto);
        }

        public async Task<ApiResult<TokenResult>> RefreshAsync(RefreshDto refreshDto)
        {
            return await SendAsync<TokenResult>(HttpMethod.Post, ApiRoutes.Users.Refresh, refreshDto);
        }

    }
}
