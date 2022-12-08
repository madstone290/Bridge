using Bridge.Application.Common;
using Bridge.Application.Common.Exceptions.EntityNotFoundExceptions;
using Bridge.Application.Users;
using Bridge.Domain.Places.Repos;
using Bridge.Domain.Products.Entities;
using Bridge.Domain.Products.Enums;
using Bridge.Domain.Products.Repos;

namespace Bridge.Application.Products.Commands
{
    public class CreateProductCommand : ICommand<Guid>
    {
        /// <summary>
        /// 사용자
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// 제품명
        /// </summary>
        public string Name { get;  set; } = string.Empty;

        /// <summary>
        /// 제품이 판매되는 장소
        /// </summary>
        public Guid PlaceId { get; set; }

        /// <summary>
        /// 제품 가격
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// 제품 범주
        /// </summary>
        public List<ProductCategory> Categories { get; set; } = new();

    }

    public class CreateProductCommandHandler : CommandHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly IPlaceRepository _placeRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IProductRepository productRepository, IPlaceRepository placeRepository, IUnitOfWork unitOfWork, IUserService userService)
        {
            _productRepository = productRepository;
            _placeRepository = placeRepository;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public override async Task<Guid> HandleCommand(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var place = await _placeRepository.FindByIdAsync(command.PlaceId) ?? throw new PlaceNotFoundException(new { command.PlaceId });
            await PermissionChecker.ThrowIfNoPermission(place, command.UserId, _userService);

            Product product = new Product(command.UserId, command.Name, place);
            product.SetPrice(command.Price);

            foreach (var category in command.Categories)
                product.AddCategory(category);

            await _productRepository.AddAsync(product);
            await _unitOfWork.CommitAsync();
            return product.Id;
        }
    }
}
