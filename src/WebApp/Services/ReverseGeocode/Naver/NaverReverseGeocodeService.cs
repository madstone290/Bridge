using Bridge.Shared;

namespace Bridge.WebApp.Services.ReverseGeocode.Naver
{
    public class NaverReverseGeocodeService : IReverseGeocodeService
    {
        private readonly NaverReverseGeocodeApi _api;

        public NaverReverseGeocodeService(NaverReverseGeocodeApi api)
        {
            _api = api;
        }

        public async Task<Result<string>> GetAddressAsync(double latitude, double longitude)
        {
            var result = await _api.GetAddressResult(latitude, longitude);
            if (!result.Success)
                return Result<string>.FailResult(result.Error);

            var responseBody = result.Data!;

            if (!responseBody.Results.Any())
                return Result<string>.FailResult("주소 검색결과가 없습니다");

            var region = responseBody.Results[0].Region;
            return Result<string>.SuccessResult($"{region.Area1.Name} {region.Area2.Name} {region.Area3.Name} {region.Area4.Name}".Trim());
        }
    }
}
