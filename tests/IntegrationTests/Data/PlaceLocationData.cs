using Bridge.Domain.Common.ValueObjects;

namespace Bridge.IntegrationTests.Data
{
    /// <summary>
    /// 주요 장소의 위치정보
    /// </summary>
    public static class PlaceLocationData
    {
        public static PlaceLocation Daegu { get; } = PlaceLocation.Create(35.871433, 128.601440, 1099432, 1764433);
        public static PlaceLocation Seoul { get; } = PlaceLocation.Create(37.566536, 126.977966, 953898, 1952036);
        public static PlaceLocation Busan { get; } = PlaceLocation.Create(35.179554, 129.075638, 1143470, 1688276);
        public static PlaceLocation Gwangju { get; } = PlaceLocation.Create(35.162441, 126.910339, 946298, 1685401);
        public static PlaceLocation Daejeon { get; } = PlaceLocation.Create(36.335041, 127.400208, 991044, 1815300);
        public static PlaceLocation Gangneung { get; } = PlaceLocation.Create(37.743571, 128.876495, 1121275, 1972441);
        public static PlaceLocation Pohang { get; } = PlaceLocation.Create(35.998008, 129.366760, 1168260, 1779523);

    }
}
