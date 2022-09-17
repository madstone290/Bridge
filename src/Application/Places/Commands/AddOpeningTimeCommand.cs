using Bridge.Application.Common;
using Bridge.Application.Common.Exceptions.EntityNotFoundExceptions;
using Bridge.Application.Places.Dtos;
using Bridge.Domain.Places.Repos;
using MediatR;

namespace Bridge.Application.Places.Commands
{
    public class AddOpeningTimeCommand : ICommand
    {
        /// <summary>
        /// 장소 아이디
        /// </summary>
        public long PlaceId { get; set; }

        /// <summary>
        /// 영업시간
        /// </summary>
        public OpeningTimeDto OpeningTime { get; set; } = new();
    }

    public class AddOpeningTimeCommandHandler : CommandHandler<AddOpeningTimeCommand, Unit>
    {
        private readonly IPlaceRepository _placeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddOpeningTimeCommandHandler(IPlaceRepository placeRepository, IUnitOfWork unitOfWork)
        {
            _placeRepository = placeRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task<Unit> HandleCommand(AddOpeningTimeCommand command, CancellationToken cancellationToken)
        {
            var place = await _placeRepository.FindByIdAsync(command.PlaceId) ?? throw new PlaceNotFoundException(new { command.PlaceId });

            place.AddOpeningTime(
                command.OpeningTime.Day,
                command.OpeningTime.OpenTime,
                command.OpeningTime.CloseTime,
                command.OpeningTime.BreakStartTime,
                command.OpeningTime.BreakEndTime);

            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
