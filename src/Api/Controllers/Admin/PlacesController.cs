using Bridge.Application.Common;
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
        [Route(ApiRoutes.Admin.Places.GetPaginatedList)]
        [ProducesResponseType(typeof(PaginatedList<PlaceReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlaces([FromQuery] PlaceType? placeType, [FromQuery] string? searchText, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            var query = new GetPlacesPaginationQuery()
            {
                SearchText = searchText,
                PlaceType = placeType,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
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

        [HttpPut]
        [Route(ApiRoutes.Admin.Places.UpdateBaseInfo)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePlaceBaseInfo([FromRoute] long id, [FromBody] UpdatePlaceBaseInfoCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return Ok();
        }


        [HttpPut]
        [Route(ApiRoutes.Admin.Places.UpdateOpeningTimes)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePlaceOpeningTimes([FromRoute] long id, [FromBody] UpdatePlaceOpeningTimesCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        [Route(ApiRoutes.Admin.Places.UpdateCategories)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCategories([FromRoute] long id, [FromBody] UpdatePlaceCategoryCommand command)
        {
            command.PlaceId = id;
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        [Route(ApiRoutes.Admin.Places.Close)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ClosePlace([FromRoute] long id)
        {
            var command = new ClosePlaceCommand() { Id = id };
            await _mediator.Send(command);
            return Ok();
        }

    }
}
