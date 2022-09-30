using Bridge.Application.Common;
using Bridge.Application.Common.Services;
using Bridge.Application.Places.Dtos;
using Bridge.Domain.Common.ValueObjects;
using Bridge.Domain.Places.Entities;
using Bridge.Domain.Places.Repos;

namespace Bridge.Application.Places.Commands
{
    /// <summary>
    /// 장소를 생성한다
    /// </summary>
    public class CreatePlaceCommand : ICommand<long>
    {
        /// <summary>
        /// 장소유형
        /// </summary>
        public PlaceType Type { get; set; }

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
        public List<OpeningTimeDto> OpeningTimes { get; set; } = new();
    }

    public class CreatePlaceCommandHandler : CommandHandler<CreatePlaceCommand, long>
    {
        private readonly IAddressMapService _addressMapService;
        private readonly IPlaceRepository _placeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePlaceCommandHandler(IAddressMapService addressMapService,
                                         IPlaceRepository placeRepository,
                                         IUnitOfWork unitOfWork)
        {
            _addressMapService = addressMapService;
            _placeRepository = placeRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task<long> HandleCommand(CreatePlaceCommand command, CancellationToken cancellationToken)
        {
            var latitudeLongitude = await _addressMapService.GetLatitudeAndLongitudeAsync(command.Address.RoadAddress);
            var eatingNorthing = await _addressMapService.GetUTM_K_EastingAndNorthingAsync(command.Address.RoadAddress);

            var location = PlaceLocation.Create(latitudeLongitude.Item1, latitudeLongitude.Item2, eatingNorthing.Item1, eatingNorthing.Item2);

            // todo address service 적용
            var address = Address.Create(command.Address.RoadAddress, string.Empty, command.Address.Details, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            var place = Place.Create(command.Type, command.Name, address, location);
            place.SetContactNumber(command.ContactNumber);

            foreach (var category in command.Categories)
                place.AddCategory(category);

            foreach (var openingTimeDto in command.OpeningTimes)
            {
                if (openingTimeDto.OpenTime.HasValue && openingTimeDto.CloseTime.HasValue)
                    place.SetOpenCloseTime(openingTimeDto.Day, openingTimeDto.OpenTime.Value, openingTimeDto.CloseTime.Value);

                if (openingTimeDto.BreakStartTime.HasValue && openingTimeDto.BreakEndTime.HasValue)
                    place.SetBreakTime(openingTimeDto.Day, openingTimeDto.BreakStartTime.Value, openingTimeDto.BreakEndTime.Value);

                if (openingTimeDto.Dayoff)
                    place.SetDayoff(openingTimeDto.Day);

                if (openingTimeDto.TwentyFourHours)
                    place.SetTwentyFourHours(openingTimeDto.Day);
            }

            await _placeRepository.AddAsync(place);

            await _unitOfWork.CommitAsync();
            return place.Id;
        }
    }
}
