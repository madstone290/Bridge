using Bridge.Api.Constants;
using Bridge.Application.Products.Commands;
using Bridge.Application.Products.Queries;
using Bridge.Application.Products.ReadModels;
using Bridge.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bridge.Api.Controllers
{
    public class ProductsController : ApiController
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route(ApiRoutes.Products.Get)]
        [ProducesResponseType(typeof(ProductReadModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProduct([FromRoute] long id)
        {
            var query = new GetProductByIdQuery()
            {
                Id = id
            };
            var place = await _mediator.Send(query);
            return Ok(place);
        }

        [HttpGet]
        [Route(ApiRoutes.Products.GetList)]
        [ProducesResponseType(typeof(List<ProductReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProducts([FromQuery] long placeId)
        {
            var query = new GetProductsByPlaceIdQuery()
            {
                PlaceId = placeId
            };
            var place = await _mediator.Send(query);
            return Ok(place);
        }

        [Authorize(Policy = PolicyConstants.AdminOrProvider)]
        [HttpPost]
        [Route(ApiRoutes.Products.Create)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            var placeId = await _mediator.Send(command);
            return Ok(placeId);
        }

        [Authorize(Policy = PolicyConstants.AdminOrProvider)]
        [HttpPut]
        [Route(ApiRoutes.Products.Update)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProduct([FromRoute] long id, [FromBody] UpdateProductCommand command)
        {
            command.ProductId = id;
            await _mediator.Send(command);
            return Ok();
        }



    }
}
