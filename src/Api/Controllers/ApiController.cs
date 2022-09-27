using Bridge.Api.ActionFilters;
using Microsoft.AspNetCore.Mvc;

namespace Bridge.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ServiceFilter(typeof(ExceptionFilter))]
    public class ApiController : ControllerBase
    {

    }
}
