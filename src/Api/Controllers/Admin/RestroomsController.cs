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
    [Authorize(Policy = PolicyConstants.Admin)]
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
        public async Task<IActionResult> GetPlace([FromRoute] long id)
        {
            var query = new GetRestroomByIdQuery() { Id = id };
            var place = await _mediator.Send(query);
            return Ok(place);
        }

        [HttpPost]
        [Route(ApiRoutes.Admin.Restrooms.Create)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateRestroom([FromBody] CreateRestroomCommand command)
        {
            var placeId = await _mediator.Send(command);
            return Ok(placeId);
        }

        [HttpPost]
        [Route(ApiRoutes.Admin.Restrooms.CreateBatch)]
        [ProducesResponseType( StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateRestroom([FromBody] CreateRestroomBatchCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        [Route(ApiRoutes.Admin.Restrooms.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateRestroom([FromRoute] long id, [FromBody] UpdateRestroomCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return Ok();
        }

    }
}
