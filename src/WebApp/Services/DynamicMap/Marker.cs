namespace Bridge.WebApp.Services.DynamicMap
{
    public class Marker
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public double Latitude { get; init; }
        public double Longitude { get; init; }
    }

}
