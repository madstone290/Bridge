using Bridge.Infrastructure.Identity;
using Bridge.Shared;
using Bridge.Shared.ApiContract.Dtos.Identity;
using System.Net.Http.Json;

namespace Bridge.IntegrationTests.Config
{
    public class TestClient
    {
        public HttpClient Client { get; }
        public string AdminAccessToken { get; }
        public string ConsumerAccessToken { get; }

        public TestClient(HttpClient client)
        {
            Client = client;

            var adminTokenResult = LoginAsync(TestUser.Admin.Email, TestUser.Admin.Password).GetAwaiter().GetResult();
            AdminAccessToken = adminTokenResult.AccessToken;

            var consumerTokenResult = LoginAsync(TestUser.Consumer.Email, TestUser.Consumer.Password).GetAwaiter().GetResult();
            ConsumerAccessToken = consumerTokenResult.AccessToken;
        }

        /// <summary>
        /// 관리자 인증으로 요청 전송
        /// </summary>
        public async Task<HttpResponseMessage> SendAsAdminAsync(HttpRequestMessage request)
        {
            SetBearerToken(request, AdminAccessToken);
            return await Client.SendAsync(request);
        }

        /// <summary>
        /// 소비자 인증으로 요청 전송
        /// </summary>
        public Task<HttpResponseMessage> SendAsConsumerAsync(HttpRequestMessage request)
        {
            SetBearerToken(request, ConsumerAccessToken);
            return Client.SendAsync(request);
        }

        /// <summary>
        /// 제공된 사용자로 인증후 요청 전송
        /// </summary>
        public async Task<HttpResponseMessage> SendAsUser(string email, string password, HttpRequestMessage request)
        {
            var tokenResult = await LoginAsync(email, password);
            SetBearerToken(request, tokenResult.AccessToken);
            return await Client.SendAsync(request);
        }

        /// <summary>
        /// 인증없이 요청 전송
        /// </summary>
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return Client.SendAsync(request);
        }

        private async Task<RefreshResult> LoginAsync(string email, string password)
        {
            var loginDto = new LoginDto()
            {
                Email = email,
                Password = password
            };
            var response = await Client.PostAsJsonAsync(ApiRoutes.Users.Login, loginDto);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"로그인 실패 {response.StatusCode}");
            var result = await response.Content.ReadFromJsonAsync<RefreshResult>();
            if (result == null)
                throw new Exception("로그인 응답 컨텐츠가 없습니다");
            return result;
        }

        private void SetBearerToken(HttpRequestMessage request, string token)
        {
            request.Headers.Remove("Authorization");
            request.Headers.Add("Authorization", $"bearer {token}");
        }
    }
}
