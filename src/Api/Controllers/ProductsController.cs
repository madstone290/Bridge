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


        [HttpPost]
        [Route(ApiRoutes.Products.Search)]
        [ProducesResponseType(typeof(List<ProductReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromBody] SearchProductsQuery query)
        {
            var places = await _mediator.Send(query);
            return Ok(places);
        }
    }
}
