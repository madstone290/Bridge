using Bridge.Application.Common;
using Bridge.Application.Common.Exceptions.EntityNotFoundExceptions;
using Bridge.Domain.Products.Repos;
using MediatR;

namespace Bridge.Application.Products.Commands
{
    /// <summary>
    /// 제품을 폐기한다
    /// </summary>
    public class DiscardProductCommand : ICommand
    {
        /// <summary>
        /// 제품 아이디
        /// </summary>
        public Guid Id { get; set; }
    }

    public class DiscardProductCommandHandler : CommandHandler<DiscardProductCommand, Unit>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DiscardProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task<Unit> HandleCommand(DiscardProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _productRepository.FindByIdAsync(command.Id) ?? throw new ProductNotFoundException(new { command.Id });
            product.Discard();
         
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
