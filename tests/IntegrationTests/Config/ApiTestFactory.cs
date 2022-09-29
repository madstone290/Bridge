using Bridge.Api.Controllers.Identity.Dtos;
using Bridge.Application.Common.Services;
using Bridge.Infrastructure.Data;
using Bridge.Infrastructure.Identity.Services;
using Bridge.Infrastructure.Services;
using Bridge.IntegrationTests.Config.ApiClients;
using Bridge.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;

namespace Bridge.IntegrationTests.Config
{
    /**
     * 동일한 클라이언트를 이용하여 테스트를 실행한다.
     * 병렬실행을 위해 ClassFixture를 적용한다.
     */

    /// <summary>
    /// API테스트를 위한 팩토리.
    /// 테스트를 위한 서버 및 클라이언트를 구축한다.
    /// </summary>
    public class ApiTestFactory
    {
        public TestClient Client { get; }

        public ApiClient ApiClient { get; }

        public ApiTestFactory()
        {
            Client = testClient;
            ApiClient =  apiClient;
        }

        private static TestClient testClient;
        private static ApiClient apiClient;
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

                      // replace IAdminUserService
                      services.RemoveAll<IAdminUserService>();
                      services.AddScoped<IAdminUserService, TestAdminUserService>();

                      // replace IEmailVerificationService
                      services.RemoveAll<IEmailVerificationService>();
                      services.AddScoped<IEmailVerificationService, TestEmailVerificationService>();

                  });
              });

            
            using var scope = webApplicationFactory.Services.CreateScope();

            // DB 마이그레이션
            var bridgeContext = scope.ServiceProvider.GetRequiredService<BridgeContext>();
            bridgeContext.Database.Migrate();


            testClient = new TestClient(webApplicationFactory.CreateClient());

            var placeApiClient = new PlaceApiClient(testClient);
            var productApiClient = new ProductApiClient(testClient);
            apiClient = new ApiClient(placeApiClient, productApiClient);

            // 시드 생성
            foreach (var user in Seeds.TestUsers)
            {
                testClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, ApiRoutes.Users.Register)
                {
                    Content = JsonContent.Create(new UserDto()
                    {
                        Email = user.Email,
                        Password = user.Password
                    })
                });
            }
        }


    }

}
