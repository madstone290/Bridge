using Bridge.Application.Places.Commands;
using Bridge.Application.Places.Queries;
using Bridge.Application.Places.ReadModels;
using Bridge.Domain.Places.Entities;
using Bridge.Shared;
using Bridge.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bridge.Api.Controllers.Admin
{
    [Tags("Admin Places")]
    [Authorize(Policy = PolicyConstants.Admin)]
    public class PlacesController : ApiController
    {
        private readonly IMediator _mediator;

        public PlacesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route(ApiRoutes.Admin.Places.Get)]
        [ProducesResponseType(typeof(PlaceReadModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlace([FromRoute] long id)
        {
            var query = new GetPlaceByIdQuery()
            {
                Id = id
            };
            var place = await _mediator.Send(query);
            return Ok(place);
        }

        [HttpGet]
        [Route(ApiRoutes.Admin.Places.GetList)]
        [ProducesResponseType(typeof(List<PlaceReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlaces([FromQuery] PlaceType placeType)
        {
            var query = new GetPlacesByPlaceTypeQuery() { PlaceType = placeType };
            var places = await _mediator.Send(query);
            return Ok(places);
        }

        [HttpPost]
        [Route(ApiRoutes.Admin.Places.Search)]
        [ProducesResponseType(typeof(List<PlaceReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchPlaces([FromBody] SearchPlacesQuery query)
        {
            var places = await _mediator.Send(query);
            return Ok(places);
        }

        [HttpPost]
        [Route(ApiRoutes.Admin.Places.Create)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreatePlace([FromBody] CreatePlaceCommand command)
        {
            var placeId = await _mediator.Send(command);
            return Ok(placeId);
        }

        [HttpPut]
        [Route(ApiRoutes.Admin.Places.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePlace([FromRoute] long id, [FromBody] UpdatePlaceCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost]
        [Route(ApiRoutes.Admin.Places.AddOpeningTime)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddOpeningTime([FromRoute] long id, [FromBody] AddOpeningTimeCommand command)
        {
            command.PlaceId = id;
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost]
        [Route(ApiRoutes.Admin.Places.UpdateCategories)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddCategory([FromRoute] long id, [FromBody] UpdatePlaceCategoryCommand command)
        {
            command.PlaceId = id;
            await _mediator.Send(command);
            return Ok();
        }


    }
}
