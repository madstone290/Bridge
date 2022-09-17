using Bridge.Domain.Users.Entities;
using Bridge.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

                  });
              });

            // 슈퍼유저 생성
            using var scope = webApplicationFactory.Services.CreateScope();

            var bridgeContext = scope.ServiceProvider.GetRequiredService<BridgeContext>();
            if(!bridgeContext.Users.Any(x=> x.IdentityUserId == Seeds.RootUser.IdentityUserId))
                bridgeContext.Users.Add(Seeds.RootUser);
            bridgeContext.SaveChanges();
        }

        
    }

}
