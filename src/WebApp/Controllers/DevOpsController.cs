using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Bridge.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DevOpsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DevOpsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Blue/Green 배포전략을 위해 사용한다.
        /// 현재 배포중인 컬러를 반환한다.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Color")]
        public string GetDeploymentColor()
        {
            return _configuration.GetValue<string>("DevOpsColor") ?? "n/a";
        }

        [HttpGet]
        [Route("Version")]
        public string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;
        }
    }
}
