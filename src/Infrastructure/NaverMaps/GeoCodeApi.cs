using Bridge.Infrastructure.NaverMaps.Data;
using Bridge.Shared.Extensions;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace Bridge.Infrastructure.NaverMaps
{
    public class GeoCodeApi
    {
        public class Config
        {
            [Required]
            public string ClientId { get; set; } = string.Empty;

            [Required]
            public string ClientSecret { get; set; } = string.Empty;
        }

        /// <summary>
        /// api 요청 주소
        /// </summary>
        private const string URI = "https://naveropenapi.apigw.ntruss.com/map-geocode/v2/geocode";
        private const string CLIENT_ID_HEADER = "X-NCP-APIGW-API-KEY-ID";
        private const string CLIENT_SECRET_HEADER = "X-NCP-APIGW-API-KEY";

        private readonly HttpClient _httpClient;
        private readonly Config _config;

        public GeoCodeApi(HttpClient httpClient, IOptions<Config> configOptions)
        {
            _httpClient = httpClient;
            _config = configOptions.Value;
        }

        public async Task<GeoCodeResponseBody> GetAddressInfo(string address)
        {
            if(string.IsNullOrEmpty(address))
                throw new ArgumentNullException(nameof(address));

            var request = new HttpRequestMessage(HttpMethod.Get, URI.AddQueryParam("query", address));
            request.Headers.Add(CLIENT_ID_HEADER, _config.ClientId);
            request.Headers.Add(CLIENT_SECRET_HEADER, _config.ClientSecret);

            var response = await _httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadFromJsonAsync<GeoCodeResponseBody>() 
                ?? new GeoCodeResponseBody() { Status = "Error", ErrorMessage = "Failed to parsing" };
            return responseBody;
        }
    }
}

