using Bridge.Application.Common.Services;
using Bridge.Infrastructure.Data;
using Bridge.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Linq;

namespace Bridge.IntegrationTests.Config
{
    /// <summary>
    /// API테스트를 위한 팩토리.
    /// 테스트를 위한 서버 및 클라이언트를 구축한다.
    /// </summary>
    public class ApiTestFactory
    {
        public HttpClient Client { get; }

        public ApiTestFactory()
        {
            Client = webApplicationFactory.CreateClient();
        }

        private static readonly WebApplicationFactory<Program> webApplicationFactory;

        static ApiTestFactory()
        {
            webApplicationFactory = new WebApplicationFactory<Program>()
              .WithWebHostBuilder(webHostBuilder =>
              {
                  webHostBuilder.ConfigureTestServices(services =>
                  {

                      // replace BridgeContext
                      services.RemoveAll<DbContextOptions<BridgeContext>>();

                      var jsonString = File.ReadAllText("Secrets/bridge_test_secret.json");
                      var jObj = JObject.Parse(jsonString);
                      var connectionStringObj = jObj["ConnectionString"];
                      if (connectionStringObj == null)
                          throw new Exception("DB 연결 문자열 불러오기에 실패하였습니다");

                      var connectionString = connectionStringObj.ToString();
                      services.AddDbContext<BridgeContext>(options =>
                      {
                          options.UseNpgsql(connectionString);
                      });

                      // replace IAddressMapService
                      services.RemoveAll<IAddressMapService>();
                      services.AddScoped<IAddressMapService, DemoAddressMapService>();

                  });
              });

            
            using var scope = webApplicationFactory.Services.CreateScope();

            // DB 마이그레이션
            var bridgeContext = scope.ServiceProvider.GetRequiredService<BridgeContext>();
            bridgeContext.Database.Migrate();

            // 슈퍼유저 생성

            bridgeContext.SaveChanges();
        }


    }

}
