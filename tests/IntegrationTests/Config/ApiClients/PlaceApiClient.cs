using Bridge.Application.Places.Commands;
using Bridge.Shared;
using System.Net.Http.Json;

namespace Bridge.IntegrationTests.Config.ApiClients
{
    public class PlaceApiClient
    {
        public TestClient Client { get; }

        public PlaceApiClient(TestClient client)
        {
            Client = client;
        }

        public async Task<long> CreatePlaceAsync(CreatePlaceCommand command)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Places.Create)
            {
                Content = JsonContent.Create(command)
            };
            var response = await Client.SendAsAdminAsync(request);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"요청 실패 {response.StatusCode}");
            return await response.Content.ReadFromJsonAsync<long>();
        }
    }
}
