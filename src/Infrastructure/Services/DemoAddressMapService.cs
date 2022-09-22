using Bridge.Application.Common.Services;

namespace Bridge.Infrastructure.Services
{
    /// <summary>
    /// 미리 정의된 동작으로 주소의 지리정보를 불러온다.
    /// </summary>
    public class DemoAddressMapService : IAddressMapService
    {
        public Func<string, Tuple<double, double>> GetLatitudeAndLongitudeFunc { get; set; } = 
            (address) => new Tuple<double, double>(32.3333, 127.6666);


        public Func<string, Tuple<double, double>> GetUTM_K_EastingAndNorthingFunc { get; set; } =
            (address) => new Tuple<double, double>(960447, 1945318);

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
