using Bridge.Application.Users.Commands;
using Bridge.Application.Users.Queries;
using Bridge.Application.Users.ReadModels;
using Bridge.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bridge.Api.Controllers
{
    public class AdminUsersController : ApiController
    {
        private readonly IMediator _mediator;

        public AdminUsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route(ApiRoutes.AdminUsers.Get)]
        [ProducesResponseType(typeof(UserReadModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAdminUser([FromRoute] long id)
        {
            var query = new GetAdminUserByIdQuery() 
            { 
                Id = id
            };
            var adminUser = await _mediator.Send(query);
            return Ok(adminUser);
        }

        [HttpPost]
        [Route(ApiRoutes.AdminUsers.Create)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]   
        public async Task<IActionResult> CreateAdminUser([FromBody] CreateAdminUserCommand command)
        {
            var adminUserId = await _mediator.Send(command);
            return Ok(adminUserId);
        }
    }
}
