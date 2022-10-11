using Bridge.Api.Controllers.Identity.Dtos;
using Bridge.Infrastructure.Identity;
using Bridge.Shared;
using Bridge.WebApp.Services.Identity;

namespace Bridge.WebApp.Api.ApiClients.Identity
{
    public class UserApiClient : ApiClient
    {
        public UserApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<ApiResult<long>> RegisterAsync(RegisterDto registerDto)
        {
            return await SendAsync<long>(HttpMethod.Post, ApiRoutes.Users.Register, registerDto);
        }

        public async Task<ApiResult<Void>> SendVerificationEmailAsync(VerificationDto verificationDto)
        {
            return await SendAsync<Void>(HttpMethod.Post, ApiRoutes.Users.SendVerificationEmail, verificationDto);
        }

        public async Task<ApiResult<LoginResult>> LoginAsync(LoginDto loginDto)
        {
            return await SendAsync<LoginResult>(HttpMethod.Post, ApiRoutes.Users.Login, loginDto);
        }

        public async Task<ApiResult<RefreshResult>> RefreshAsync(RefreshDto refreshDto)
        {
            return await SendAsync<RefreshResult>(HttpMethod.Post, ApiRoutes.Users.Refresh, refreshDto);
        }

    }
}
