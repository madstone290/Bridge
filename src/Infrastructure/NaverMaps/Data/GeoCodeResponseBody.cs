namespace Bridge.Infrastructure.NaverMaps.Data
{
    /// <summary>
    /// Geo Code Api 응답 바디
    /// </summary>
    public class GeoCodeResponseBody
    {
        public const string StatusOk = "OK";

        public string Status { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;

        public Meta Meta { get; set; } = Meta.Empty;

        public IEnumerable<Address> Addresses { get; set; } = Enumerable.Empty<Address>();

        public Address Address => Addresses.First();
    }
}
