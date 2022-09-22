using Bridge.Application.Common.Services;

namespace Bridge.Infrastructure.Services
{
    /// <summary>
    /// 미리 정의된 동작으로 주소의 지리정보를 불러온다.
    /// </summary>
    public class DemoAddressMapService : IAddressMapService
    {
        class UtmK_Provider
        {
            public Tuple<double, double> Provide(string address)
            {
                var utmIndex = address.IndexOf("utm:");
                if(utmIndex < 0)
                    return Tuple.Create(100000d, 200000d);

                var eastingNorthingStr = address.Substring(utmIndex + 4, address.Length - (utmIndex + 4));
                var eastingNorthing = eastingNorthingStr.Split(",");
                return Tuple.Create(double.Parse(eastingNorthing[0]), double.Parse(eastingNorthing[1]));
            }
        }

        static readonly UtmK_Provider utmK_Provider = new();

        public Func<string, Tuple<double, double>> GetLatitudeAndLongitudeFunc { get; set; } = 
            (address) => new Tuple<double, double>(32.3333, 127.6666);


        public Func<string, Tuple<double, double>> GetUTM_K_EastingAndNorthingFunc { get; set; } =
            (address) => utmK_Provider.Provide(address);

        public async Task<Tuple<double, double>> GetLatitudeAndLongitudeAsync(string address)
        {
            await Task.Delay(100);

            return GetLatitudeAndLongitudeFunc(address);
        }

        public async Task<Tuple<double, double>> GetUTM_K_EastingAndNorthingAsync(string address)
        {
            await Task.Delay(100);

            return GetUTM_K_EastingAndNorthingFunc(address);
        }
    }
}
