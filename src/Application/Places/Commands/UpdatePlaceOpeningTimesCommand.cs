using Bridge.Application.Common;
using Bridge.Application.Common.Exceptions.EntityNotFoundExceptions;
using Bridge.Application.Places.Dtos;
using Bridge.Application.Users;
using Bridge.Domain.Places.Repos;
using MediatR;

namespace Bridge.Application.Places.Commands
{
    /// <summary>
    /// 장소 영업시간을 수정한다
    /// </summary>
    public class UpdatePlaceOpeningTimesCommand : ICommand<Unit>
    {
        /// <summary>
        /// 사용자
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// 장소 아이디
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 영업시간
        /// </summary>
        public IEnumerable<OpeningTimeDto> OpeningTimes { get; set; } = Enumerable.Empty<OpeningTimeDto>();
    }

    public class UpdatePlaceOpeningTimesCommandHandler : CommandHandler<UpdatePlaceOpeningTimesCommand, Unit>
    {
        private readonly IPlaceRepository _placeRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePlaceOpeningTimesCommandHandler(IPlaceRepository placeRepository, IUnitOfWork unitOfWork, IUserService userService)
        {
            _placeRepository = placeRepository;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public override async Task<Unit> HandleCommand(UpdatePlaceOpeningTimesCommand command, CancellationToken cancellationToken)
        {
            var place = await _placeRepository.FindByIdAsync(command.Id) ?? throw new PlaceNotFoundException(new { command.Id });
            await PermissionChecker.ThrowIfNoPermission(place, command.UserId, _userService);

            foreach (var openingTimeDto in command.OpeningTimes)
            {
                if (openingTimeDto.OpenTime.HasValue && openingTimeDto.CloseTime.HasValue)
                    place.SetOpenCloseTime(openingTimeDto.Day, openingTimeDto.OpenTime.Value, openingTimeDto.CloseTime.Value);

                if (openingTimeDto.BreakStartTime.HasValue && openingTimeDto.BreakEndTime.HasValue)
                    place.SetBreakTime(openingTimeDto.Day, openingTimeDto.BreakStartTime.Value, openingTimeDto.BreakEndTime.Value);

                place.SetDayoff(openingTimeDto.Day, openingTimeDto.IsDayoff);
                place.SetTwentyFourHours(openingTimeDto.Day, openingTimeDto.Is24Hours);
            }

            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
