using Bridge.Application.Places.Queries;
using Bridge.Shared;
using Bridge.WebApp.Api.ApiClients;
using Bridge.WebApp.Pages.Home.Records;

namespace Bridge.WebApp.Pages.Home.Models
{
    public class PlaceListModel
    {
        private readonly PlaceApiClient _placeApiClient;

        public PlaceListModel(PlaceApiClient placeApiClient)
        {
            _placeApiClient = placeApiClient;
        }

        public IEnumerable<PlaceRecord> PlaceList { get; set; } = Enumerable.Empty<PlaceRecord>();

        public async Task<Result> SeachPlacesAsync(string searchText, double latitude, double longitude)
        {
            var query = new SearchPlacesQuery()
            {
                SearchText = searchText,
                Latitude = latitude,
                Longitude = longitude
            };
            var apiResult = await _placeApiClient.SearchPlaces(query);
            if (!apiResult.Success)
                return Result.FailResult(apiResult.Error);
            if (apiResult.Data == null)
                return Result.FailResult("데이터가 없습니다");

            PlaceList = apiResult.Data
                .Select(x =>
                {
                    var place = PlaceRecord.Create(x);
                    if (x.ImagePath != null)
                        place.ImageUrl = new Uri(_placeApiClient.HttpClient.BaseAddress!, x.ImagePath).ToString();
                    return place;
                })
                .OrderBy(x => x.Distance);

            return Result.SuccessResult();
        }

    }
}
