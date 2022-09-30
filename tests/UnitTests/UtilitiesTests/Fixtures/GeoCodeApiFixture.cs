using Bridge.Infrastructure.NaverMaps;
using Bridge.Shared.Extensions;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Bridge.UnitTests.UtilitiesTests.Fixtures
{
    public class GeoCodeApiFixture
    {
        public GeoCodeApiFixture()
        {
            Config = config;
        }

        public GeoCodeApi.Config Config { get; }

        private static readonly GeoCodeApi.Config config;
        static GeoCodeApiFixture()
        {
            var filePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!, "Secrets/geocode_api_config.json");
            var node = JsonObject.Parse(File.ReadAllText(filePath));
            config =  JsonSerializer.Deserialize<GeoCodeApi.Config>(node!["GeoCodeApi"]!.ToString())
                ?? throw new Exception("GeoCode 설정이 없습니다");
            
        }

    }
}
