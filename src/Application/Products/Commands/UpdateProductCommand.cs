using Bridge.Application.Common;
using Bridge.Application.Common.Exceptions.EntityNotFoundExceptions;
using Bridge.Application.Users;
using Bridge.Domain.Products.Enums;
using Bridge.Domain.Products.Repos;
using MediatR;

namespace Bridge.Application.Products.Commands
{
    public class UpdateProductCommand : ICommand
    {
        /// <summary>
        /// 사용자
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// 제품 아이디
        /// </summary>
        public Guid Id { get; set; }

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
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IUserService userService)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public override async Task<Unit> HandleCommand(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _productRepository.FindByIdAsync(command.Id) ?? throw new ProductNotFoundException(new { command.Id });
            await PermissionChecker.ThrowIfNoPermission(product, command.UserId, _userService);

            product.SetName(command.Name);
            product.SetPrice(command.Price);
            product.UpdateCategories(command.Categories);

            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
