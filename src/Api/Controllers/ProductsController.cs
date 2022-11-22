using Bridge.Application.Products.Commands;
using Bridge.Application.Products.Queries;
using Bridge.Application.Products.ReadModels;
using Bridge.Shared;
using Bridge.Shared.Constants;
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
        [Route(ApiRoutes.Products.GetList)]
        [ProducesResponseType(typeof(List<ProductReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductList([FromQuery] Guid? placeId)
        {
            List<ProductReadModel> products;
            if(placeId.HasValue)
            {
                var query = new GetProductsByPlaceIdQuery() { PlaceId = placeId.Value };
                products = await _mediator.Send(query);
            }
            else
            {
                products = new();
            }
            return Ok(products);
            
        }

        [HttpPost]
        [Route(ApiRoutes.Products.Search)]
        [ProducesResponseType(typeof(List<ProductReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromBody] SearchProductsQuery query)
        {
            var products = await _mediator.Send(query);
            return Ok(products);
        }
     
    }
}
