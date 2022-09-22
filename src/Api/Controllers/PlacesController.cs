using Bridge.Application.Places.Commands;
using Bridge.Application.Places.Queries;
using Bridge.Application.Places.ReadModels;
using Bridge.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bridge.Api.Controllers
{
    public class PlacesController : ApiController
    {
        private readonly IMediator _mediator;

        public PlacesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route(ApiRoutes.Places.Get)]
        [ProducesResponseType(typeof(PlaceReadModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlace([FromRoute] GetPlaceByIdQuery query)
        {
            var place = await _mediator.Send(query);
            return Ok(place);
        }

        [HttpGet]
        [Route(ApiRoutes.Places.GetList)]
        [ProducesResponseType(typeof(PlaceReadModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlaces([FromQuery] GetPlacesByRegionQuery query)
        {
            var place = await _mediator.Send(query);
            return Ok(place);
        }

        [HttpPost]
        [Route(ApiRoutes.Places.Create)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreatePlace([FromBody] CreatePlaceCommand command)
        {
            var placeId = await _mediator.Send(command);
            return Ok(placeId);
        }

        [HttpPost]
        [Route(ApiRoutes.Places.AddOpeningTime)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddOpeningTime([FromRoute] long Id, [FromBody] AddOpeningTimeCommand command)
        {
            command.PlaceId = Id;
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost]
        [Route(ApiRoutes.Places.UpdateCategories)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddCategory([FromRoute] long Id, [FromBody] UpdatePlaceCategoryCommand command)
        {
            command.PlaceId = Id;
            await _mediator.Send(command);
            return Ok();
        }


    }
}
