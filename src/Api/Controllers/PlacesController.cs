﻿using Bridge.Application.Places.Commands;
using Bridge.Application.Places.Queries;
using Bridge.Application.Places.ReadModels;
using Bridge.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
        [ProducesResponseType(typeof(List<PlaceReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlaces([FromQuery] string? name,
                                                   [FromQuery] double leftEasting,
                                                   [FromQuery] double rightEasting,
                                                   [FromQuery] double bottomNorthing,
                                                   [FromQuery] double topNorthing)
        {
            List<PlaceReadModel> places;
            if (name == null)
            {
                var query = new GetPlacesByRegionQuery()
                {
                    LeftEasting = leftEasting,
                    RightEasting = rightEasting,
                    BottomNorthing = bottomNorthing,
                    TopNorthing = topNorthing
                };
                places = await _mediator.Send(query);
            }
            else
            {
                var query = new GetPlacesByNameAndRegionQuery()
                {
                    Name = name,
                    LeftEasting = leftEasting,
                    RightEasting = rightEasting,
                    BottomNorthing = bottomNorthing,
                    TopNorthing = topNorthing
                };
                places = await _mediator.Send(query);
            }
            return Ok(places);
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
