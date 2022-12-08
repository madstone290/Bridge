using Bridge.Application.Common;
using Bridge.Application.Common.Exceptions.EntityNotFoundExceptions;
using Bridge.Application.Users;
using Bridge.Domain.Places.Enums;
using Bridge.Domain.Places.Repos;
using MediatR;

namespace Bridge.Application.Places.Commands
{
    public class UpdatePlaceCategoryCommand : ICommand
    {
        public string UserId { get; set; } = string.Empty;
        public Guid PlaceId { get; set; }
        public List<PlaceCategory> Categories { get; set; } = new();
    }

    public class AddPlaceCategoryCommandHandler : CommandHandler<UpdatePlaceCategoryCommand, Unit>
    {
        private readonly IPlaceRepository _placeRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public AddPlaceCategoryCommandHandler(IPlaceRepository placeRepository, IUnitOfWork unitOfWork, IUserService userService)
        {
            _placeRepository = placeRepository;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public override async Task<Unit> HandleCommand(UpdatePlaceCategoryCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var place = await _placeRepository.FindByIdAsync(command.PlaceId) ?? throw new PlaceNotFoundException(new { command.PlaceId });
                await PermissionChecker.ThrowIfNoPermission(place, command.UserId, _userService);

                place.UpdateCategories(command.Categories);

                await _unitOfWork.CommitAsync();
            }

            catch (Exception)
            {
            }

           
            return Unit.Value;
        }
    }
}
