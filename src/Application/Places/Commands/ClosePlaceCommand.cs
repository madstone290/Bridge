using Bridge.Application;
using Bridge.Application.Common;
using Bridge.Application.Common.Exceptions;
using Bridge.Application.Common.Exceptions.EntityNotFoundExceptions;
using Bridge.Application.Users;
using Bridge.Domain.Places.Repos;
using MediatR;

namespace Bridge.Application.Places.Commands
{
    /// <summary>
    /// 장소를 폐업 처리한다
    /// </summary>
    public class ClosePlaceCommand : ICommand<Unit>
    {
        /// <summary>
        /// 사용자
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// 장소 아이디
        /// </summary>
        public Guid Id { get; set; }
    }

    public class ClosePlaceCommandHandler : CommandHandler<ClosePlaceCommand, Unit>
    {
        private readonly IPlaceRepository _placeRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public ClosePlaceCommandHandler(IPlaceRepository placeRepository,
                                        IUnitOfWork unitOfWork,
                                        IUserService userService)
        {
            _placeRepository = placeRepository;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public override async Task<Unit> HandleCommand(ClosePlaceCommand command, CancellationToken cancellationToken)
        {
            var place = await _placeRepository.FindByIdAsync(command.Id) ?? throw new PlaceNotFoundException(new { command.Id });
            await PermissionChecker.ThrowIfNoPermission(place, command.UserId, _userService);

            place.CloseDown();

            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
