using Bridge.Application.Common;
using Bridge.Application.Common.Exceptions.EntityNotFoundExceptions;
using Bridge.Domain.Products.Entities;
using Bridge.Domain.Products.Repos;
using MediatR;

namespace Bridge.Application.Products.Commands
{
    public class UpdateProductCommand : ICommand
    {
        /// <summary>
        /// 제품 아이디
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 제품명
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 제품 가격
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// 제품 범주
        /// </summary>
        public List<ProductCategory> Categories { get; set; } = new();
    }

    public class UpdateProductCommandHandler : CommandHandler<UpdateProductCommand, Unit>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task<Unit> HandleCommand(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _productRepository.FindByIdAsync(command.ProductId) ?? throw new ProductNotFoundException(new { command.ProductId });

            product.SetName(command.Name);
            product.SetPrice(command.Price);
            product.UpdateCategories(command.Categories);

            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
