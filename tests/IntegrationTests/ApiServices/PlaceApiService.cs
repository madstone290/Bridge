using Bridge.Application.Places.Commands;
using Bridge.Domain.Places.Entities;
using Bridge.Shared;
using System.Net.Http.Json;

namespace Bridge.IntegrationTests.ApiServices
{
    public class PlaceApiService
    {
        /// <summary>
        /// 장소를 생성하고 아이디를 반환한다.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<long> CreatePlaceAsync(HttpClient client, CreatePlaceCommand command)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Places.Create)
            {
                Content = JsonContent.Create(command)
            };
            var response = await client.SendAsync(request);
            return await response.Content.ReadFromJsonAsync<long>();
        }

    }
}
