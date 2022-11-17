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
        public async Task<IActionResult> GetPlace([FromRoute] Guid id)
        {
            var query = new GetPlaceByIdQuery()
            {
                Id = id
            };
            var place = await _mediator.Send(query);
            return Ok(place);
        }

        [HttpPost]
        [Route(ApiRoutes.Places.Search)]
        [ProducesResponseType(typeof(List<PlaceReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchPlaces([FromBody] SearchPlacesQuery query)
        {
            var places = await _mediator.Send(query);
            return Ok(places);
        }

        [HttpPost]
        [Route(ApiRoutes.Places.Create)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddPlace([FromBody] CreatePlaceCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }
    }
}
