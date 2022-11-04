using Bridge.Application.Common;
using Bridge.Application.Common.Exceptions.EntityNotFoundExceptions;
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
        /// 장소 아이디
        /// </summary>
        public Guid Id { get; set; }
    }

    public class ClosePlaceCommandHandler : CommandHandler<ClosePlaceCommand, Unit>
    {
        private readonly IPlaceRepository _placeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ClosePlaceCommandHandler(IPlaceRepository placeRepository,
                                        IUnitOfWork unitOfWork)
        {
            _placeRepository = placeRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task<Unit> HandleCommand(ClosePlaceCommand command, CancellationToken cancellationToken)
        {
            var place = await _placeRepository.FindByIdAsync(command.Id) ?? throw new PlaceNotFoundException(new { command.Id });
            place.CloseDown();

            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
