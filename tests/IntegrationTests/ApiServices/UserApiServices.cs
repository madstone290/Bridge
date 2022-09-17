using Bridge.Application.Users.Commands;
using Bridge.IntegrationTests.Config;
using Bridge.Shared;
using System.Net.Http.Json;
using System.Xml.Linq;

namespace Bridge.IntegrationTests.ApiServices
{
    public class UserApiService
    {
        /// <summary>
        /// 관리자를 생성하고 아이디를 반환한다.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<long> CreateAdminUserAsync(HttpClient client, CreateAdminUserCommand command)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.AdminUsers.Create)
            {
                Content = JsonContent.Create(command)
            };
            var response = await client.SendAsync(request);
            return await response.Content.ReadFromJsonAsync<long>();
        }

        /// <summary>
        /// 관리자를 생성하고 아이디를 반환한다.
        /// 루트 사용자를 이용하여 관리자를 생성한다.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public async Task<long> CreateAdminUserAsync(HttpClient client)
        {
            var command = new CreateAdminUserCommand()
            {
                CreatorIdentityUserId = Seeds.RootUser.IdentityUserId,
                Name = Seeds.RootUser.Name,
            };

            return await CreateAdminUserAsync(client, command);
        }


    }
}
