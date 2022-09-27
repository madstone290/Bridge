using Bridge.Application.Common;
using Bridge.Application.Common.Exceptions.EntityNotFoundExceptions;
using Bridge.Application.Common.Services;
using Bridge.Application.Places.Dtos;
using Bridge.Domain.Common.ValueObjects;
using Bridge.Domain.Places.Entities;
using Bridge.Domain.Places.Repos;
using Bridge.Domain.Users.Repos;
using System.Net;

namespace Bridge.Application.Places.Commands
{
    /// <summary>
    /// 장소를 생성한다
    /// </summary>
    public class CreatePlaceCommand : ICommand<long>
    {
        /// <summary>
        /// 장소를 생성하는 사용자의 아이디
        /// </summary>
        public long UserId { get; set; }

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
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 장소 카테고리
        /// </summary>
        public List<PlaceCategory> Categories { get; set; } = new();

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
        private readonly IUserRepository _userRepository;
        private readonly IPlaceRepository _placeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePlaceCommandHandler(IAddressMapService addressMapService,
                                         IUserRepository userRepository,
                                         IPlaceRepository placeRepository,
                                         IUnitOfWork unitOfWork)
        {
            _addressMapService = addressMapService;
            _userRepository = userRepository;
            _placeRepository = placeRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task<long> HandleCommand(CreatePlaceCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByIdAsync(command.UserId) ?? throw new UserNotFoundException(new { command.UserId });
            
            var latitudeLongitude = await _addressMapService.GetLatitudeAndLongitudeAsync(command.Address);
            var eatingNorthing = await _addressMapService.GetUTM_K_EastingAndNorthingAsync(command.Address);

            var location = PlaceLocation.Create(latitudeLongitude.Item1, latitudeLongitude.Item2, eatingNorthing.Item1, eatingNorthing.Item2);

            var place = Place.Create(user, command.Type, command.Name, command.Address, location);
            place.SetContactNumber(command.ContactNumber);

            foreach (var category in command.Categories)
                place.AddCategory(category);

            foreach (var openingTimeDto in command.OpeningTimes)
            {
                place.AddOpeningTime(openingTimeDto.Day, openingTimeDto.OpenTime, openingTimeDto.CloseTime,
                    openingTimeDto.BreakStartTime, openingTimeDto.BreakEndTime);
            }

            await _placeRepository.AddAsync(place);

            await _unitOfWork.CommitAsync();
            return place.Id;
        }
    }
}
