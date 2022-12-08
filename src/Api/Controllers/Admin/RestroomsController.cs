using Bridge.Application.Places.Commands;
using Bridge.Application.Places.Queries;
using Bridge.Application.Places.ReadModels;
using Bridge.Shared;
using Bridge.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bridge.Api.Controllers.Admin
{
    [Tags("Admin Restroom")]
    [Authorize]
    public class RestroomsController : ApiController
    {
        private readonly IMediator _mediator;

        public RestroomsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route(ApiRoutes.Admin.Restrooms.Get)]
        [ProducesResponseType(typeof(RestroomReadModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlace([FromRoute] Guid id)
        {
            var query = new GetRestroomByIdQuery() { Id = id };
            var place = await _mediator.Send(query);
            return Ok(place);
        }

        [HttpPost]
        [Route(ApiRoutes.Admin.Restrooms.Create)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateRestroom([FromBody] CreateRestroomCommand command)
        {
            command.UserId = UserId;
            var placeId = await _mediator.Send(command);
            return Ok(placeId);
        }

        [HttpPost]
        [Route(ApiRoutes.Admin.Restrooms.CreateBatch)]
        [ProducesResponseType( StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateRestroom([FromBody] CreateRestroomBatchCommand command)
        {
            foreach (var subcommand in command.Commands)
                subcommand.UserId = UserId;

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        [Route(ApiRoutes.Admin.Restrooms.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateRestroom([FromRoute] Guid id, [FromBody] UpdateRestroomCommand command)
        {
            command.UserId = UserId;
            command.Id = id;
            await _mediator.Send(command);
            return Ok();
        }

    }
}
