using Bridge.Shared;

namespace Bridge.WebApp.Services.GeoLocation
{
    public class HtmlGeoResult : Result<GeoPoint>
    {
        public HtmlGeoResult(GeoPoint point)
        {
            Data = point;
            Success = true;
        }

        public HtmlGeoResult(GeoError geoError)
        {
            GeoError = geoError;
            Error = geoError.FriendlyMessage;
            Success = false;
        }

        public HtmlGeoResult(string error)
        {
            Error = error;
            Success = false;
        }

        public GeoError? GeoError { get; }
    }

}
