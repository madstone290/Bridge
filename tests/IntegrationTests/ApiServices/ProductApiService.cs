using Bridge.Application.Products.Commands;
using Bridge.Application.Products.Queries;
using Bridge.Application.Products.ReadModels;
using Bridge.Shared;
using System.Net.Http.Json;

namespace Bridge.IntegrationTests.ApiServices
{
    public class ProductApiService
    {
        /// <summary>
        /// 제품을 생성하고 아이디를 반환한다.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<long> CreateProductAsync(HttpClient client, CreateProductCommand command)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Products.Create)
            {
                Content = JsonContent.Create(command)
            };
            var response = await client.SendAsync(request);
            return await response.Content.ReadFromJsonAsync<long>();
        }

        public async Task<ProductReadModel?> GetProductAsync(HttpClient client, GetProductByIdQuery query)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, ApiRoutes.Products.Get.Replace("{id}", $"{query.Id}"));
            var response = await client.SendAsync(request);
            return await response.Content.ReadFromJsonAsync<ProductReadModel>();
        }

    }
}
