using Bridge.Application.Common;
using Bridge.Application.Common.Exceptions.EntityNotFoundExceptions;
using Bridge.Application.Users;
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
        /// 사용자
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// 제품 아이디
        /// </summary>
        public Guid Id { get; set; }
    }

    public class DiscardProductCommandHandler : CommandHandler<DiscardProductCommand, Unit>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public DiscardProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IUserService userService)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public override async Task<Unit> HandleCommand(DiscardProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _productRepository.FindByIdAsync(command.Id) ?? throw new ProductNotFoundException(new { command.Id });
            await PermissionChecker.ThrowIfNoPermission(product, command.UserId, _userService);

            product.Discard();
         
            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
