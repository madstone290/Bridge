using Bridge.Application.Products.Commands;
using Bridge.Application.Products.Queries;
using Bridge.Application.Products.ReadModels;
using Bridge.Shared;
using System.Net.Http.Json;

namespace Bridge.IntegrationTests.Config.ApiClients
{
    public class AdminProductApiClient
    {
        public TestClient Client { get; }

        public AdminProductApiClient(TestClient client)
        {
            Client = client;
        }

        /// <summary>
        /// 제품을 생성하고 아이디를 반환한다.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<long> CreateProductAsync(CreateProductCommand command)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Admin.Products.Create)
            {
                Content = JsonContent.Create(command)
            };
            var response = await Client.SendAsAdminAsync(request);
            return await response.Content.ReadFromJsonAsync<long>();
        }

        public async Task<ProductReadModel?> GetProductAsync(GetProductByIdQuery query)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Admin.Products.Get.Replace("{id}", $"{query.Id}"));
            var response = await Client.SendAsAdminAsync(request);
            return await response.Content.ReadFromJsonAsync<ProductReadModel>();
        }
    }
}
