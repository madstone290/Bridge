using Microsoft.AspNetCore.Mvc;

namespace Bridge.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class ApiController : ControllerBase
    {

    }
}
