using Bridge.Application.Common;
using Bridge.Application.Common.Exceptions.EntityNotFoundExceptions;
using Bridge.Domain.Places.Repos;
using Bridge.Domain.Products.Entities;
using Bridge.Domain.Products.Repos;
using Bridge.Domain.Users.Repos;

namespace Bridge.Application.Products.Commands
{
    public class CreateProductCommand : ICommand<long>
    {
        /// <summary>
        /// 제품명
        /// </summary>
        public string Name { get;  set; } = string.Empty;

        /// <summary>
        /// 제품을 생성하는 사용자의 아이디
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 제품이 판매되는 장소
        /// </summary>
        public long PlaceId { get; set; }

        /// <summary>
        /// 제품 가격
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// 제품 범주
        /// </summary>
        public List<ProductCategory> Categories { get; set; } = new();

    }

    public class CreateProductCommandHandler : CommandHandler<CreateProductCommand, long>
    {
        private readonly IProductRepository _productRepository;
        private readonly IPlaceRepository _placeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IProductRepository productRepository, IPlaceRepository placeRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _placeRepository = placeRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task<long> HandleCommand(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByIdAsync(command.UserId) ?? throw new UserNotFoundException(new { command.UserId });
            var place = await _placeRepository.FindByIdAsync(command.PlaceId) ?? throw new PlaceNotFoundException(new { command.PlaceId });

            Product product = Product.Create(user, command.Name, place);
            product.SetPrice(command.Price);

            foreach (var category in command.Categories)
                product.AddCategory(category);

            await _productRepository.AddAsync(product);
            await _unitOfWork.CommitAsync();
            return product.Id;
        }
    }
}
