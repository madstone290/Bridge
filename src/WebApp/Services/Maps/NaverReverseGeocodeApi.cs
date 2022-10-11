using Bridge.Shared.Extensions;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Bridge.WebApp.Services.Maps
{
    /// <summary>
    /// 네이버 역좌표변환 API
    /// <para>https://api.ncloud-docs.com/docs/ai-naver-mapsreversegeocoding-gc</para>
    /// </summary>
    public class NaverReverseGeocodeApi
    {
        /// <summary>
        /// API 설정
        /// </summary>
        public class Config
        {
            [Required]
            public string ClientId { get; init; } = string.Empty;

            [Required]
            public string ClientSecret { get; init; } = string.Empty;
        }

        /// <summary>
        /// 지역 명칭
        /// </summary>
        public class Area
        {
            public string? Name { get; init; }
        }

        /// <summary>
        /// 지역명칭 정보
        /// </summary>
        public class Region
        {
            public Area Area0 { get; init; } = null!;
            public Area Area1 { get; init; } = null!;
            public Area Area2 { get; init; } = null!;
            public Area Area3 { get; init; } = null!;
            public Area Area4 { get; init; } = null!;
        }

        /// <summary>
        /// 응답데이터 개별 결과 항목
        /// </summary>
        public class Result
        {
            public string Name { get; init; } = null!;
            public Region Region { get; init; } = null!;
        }
        /// <summary>
        /// ReverseGeocode 응답 바디.
        /// Status항목을 무시하고 Results항목만 변환한다.
        /// </summary>
        public class ResponseBody
        {
            public List<Result> Results { get; init; } = null!;
        }

        /// <summary>
        /// api 요청 주소
        /// </summary>
        private const string URI = "https://naveropenapi.apigw.ntruss.com/map-reversegeocode/v2/gc";
        private const string CLIENT_ID_HEADER = "X-NCP-APIGW-API-KEY-ID";
        private const string CLIENT_SECRET_HEADER = "X-NCP-APIGW-API-KEY";

        private readonly HttpClient _httpClient;
        private readonly Config _config;

        public NaverReverseGeocodeApi(HttpClient httpClient, IOptions<Config> configOptions)
        {
            _httpClient = httpClient;
            _config = configOptions.Value;
        }

        /// <summary>
        /// 위경도에서 행정리 주소로 변환된 결과를 반환한다
        /// </summary>
        /// <param name="latitude">위도</param>
        /// <param name="longitude">경도</param>
        /// <returns>응답바디</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Bridge.Shared.Result<ResponseBody>> GetAddressResult(double latitude, double longitude)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, URI
                .AddQueryParam("coords", $"{longitude},{latitude}")
                .AddQueryParam("output", "json")
                .AddQueryParam("orders", "admcode"));
            request.Headers.Add(CLIENT_ID_HEADER, _config.ClientId);
            request.Headers.Add(CLIENT_SECRET_HEADER, _config.ClientSecret);

            var response = await _httpClient.SendAsync(request);
            var body = await response.Content.ReadFromJsonAsync<ResponseBody>();
            
            if (body == null)
                return Bridge.Shared.Result<ResponseBody>.FailResult("응답 바디 변환에 실패하였습니다");

            return Bridge.Shared.Result<ResponseBody>.SuccessResult(body);
        }

    }
}
