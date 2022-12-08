using Bridge.Application.Common;
using Bridge.Application.Products.Commands;
using Bridge.Application.Products.Queries;
using Bridge.Application.Products.ReadModels;
using Bridge.Shared;
using Bridge.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bridge.Api.Controllers.Admin
{
    [Tags("Admin Products")]
    [Authorize]
    public class ProductsController : ApiController
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route(ApiRoutes.Admin.Products.Get)]
        [ProducesResponseType(typeof(ProductReadModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProduct([FromRoute] Guid id)
        {
            var query = new GetProductByIdQuery()
            {
                Id = id
            };
            var place = await _mediator.Send(query);
            return Ok(place);
        }

        [HttpGet]
        [Route(ApiRoutes.Admin.Products.GetPaginatedList)]
        [ProducesResponseType(typeof(PaginatedList<ProductReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProducts([FromQuery] Guid placeId, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            var query = new GetProductsPaginationQuery()
            {
                PlaceId = placeId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var place = await _mediator.Send(query);
            return Ok(place);
        }

        [HttpGet]
        [Route(ApiRoutes.Admin.Products.GetList)]
        [ProducesResponseType(typeof(List<ProductReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProducts([FromQuery] Guid placeId)
        {
            var query = new GetProductsByPlaceIdQuery()
            {
                PlaceId = placeId
            };
            var place = await _mediator.Send(query);
            return Ok(place);
        }

        [HttpPost]
        [Route(ApiRoutes.Admin.Products.Create)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            command.UserId = UserId;
            var placeId = await _mediator.Send(command);
            return Ok(placeId);
        }

        [HttpPut]
        [Route(ApiRoutes.Admin.Products.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateProductCommand command)
        {
            command.UserId = UserId;
            command.Id = id;
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        [Route(ApiRoutes.Admin.Products.Discard)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Discard([FromRoute] Guid id)
        {
            var command = new DiscardProductCommand()
            { 
                UserId = UserId,
                Id = id 
            };
            await _mediator.Send(command);
            return Ok();
        }


    }
}
