using Bridge.Application.Common;
using Bridge.Application.Common.Exceptions.EntityNotFoundExceptions;
using Bridge.Application.Common.Services;
using Bridge.Application.Places.Dtos;
using Bridge.Domain.Places.Entities;
using Bridge.Domain.Places.Repos;
using MediatR;

namespace Bridge.Application.Places.Commands
{
    /// <summary>
    /// 장소를 수정한다
    /// </summary>
    public class UpdatePlaceCommand : ICommand<Unit>
    {
        /// <summary>
        /// 장소 아이디
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 장소명
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 주소
        /// </summary>
        public AddressDto Address { get; set; } = AddressDto.Empty;

        /// <summary>
        /// 장소 카테고리
        /// </summary>
        public IEnumerable<PlaceCategory> Categories { get; set; } = Enumerable.Empty<PlaceCategory>();

        /// <summary>
        /// 연락처
        /// </summary>
        public string? ContactNumber { get; set; }

        /// <summary>
        /// 영업시간
        /// </summary>
        public IEnumerable<OpeningTimeDto> OpeningTimes { get; set; } = Enumerable.Empty<OpeningTimeDto>();
    }

    public class UpdatePlaceCommandHandler : CommandHandler<UpdatePlaceCommand, Unit>
    {
        private readonly IAddressLocationService _addressLocationService;
        private readonly IPlaceRepository _placeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePlaceCommandHandler(IAddressLocationService addressMapService,
                                         IPlaceRepository placeRepository,
                                         IUnitOfWork unitOfWork)
        {
            _addressLocationService = addressMapService;
            _placeRepository = placeRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task<Unit> HandleCommand(UpdatePlaceCommand command, CancellationToken cancellationToken)
        {
            var place = await _placeRepository.FindByIdAsync(command.Id) ?? throw new PlaceNotFoundException(new { command.Id });

            place.SetName(command.Name);
            var addressLocation = await _addressLocationService.CreateAddressLocationAsync(command.Address.BaseAddress, command.Address.DetailAddress);
            place.SetAddressLocation(addressLocation.Item1, addressLocation.Item2);
            place.UpdateCategories(command.Categories);
            place.SetContactNumber(command.ContactNumber);
            foreach (var openingTimeDto in command.OpeningTimes)
            {
                if (openingTimeDto.OpenTime.HasValue && openingTimeDto.CloseTime.HasValue)
                    place.SetOpenCloseTime(openingTimeDto.Day, openingTimeDto.OpenTime.Value, openingTimeDto.CloseTime.Value);

                if (openingTimeDto.BreakStartTime.HasValue && openingTimeDto.BreakEndTime.HasValue)
                    place.SetBreakTime(openingTimeDto.Day, openingTimeDto.BreakStartTime.Value, openingTimeDto.BreakEndTime.Value);

                place.SetDayoff(openingTimeDto.Day, openingTimeDto.Dayoff);
                place.SetTwentyFourHours(openingTimeDto.Day, openingTimeDto.TwentyFourHours);
            }

            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
