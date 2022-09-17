using Bridge.Application.Common;
using Bridge.Application.Common.Exceptions;
using Bridge.Application.Common.Exceptions.EntityNotFoundExceptions;
using Bridge.Application.Places.Dtos;
using Bridge.Application.Places.ReadModels;
using Bridge.Domain.Common.ValueObjects;
using Bridge.Domain.Places.Entities;
using Bridge.Domain.Places.Repos;
using Bridge.Domain.Users.Repos;

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
        /// 장소명
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 장소 위도
        /// </summary>
        public decimal Latitude { get; set; }

        /// <summary>
        /// 장소 경도
        /// </summary>
        public decimal Longitude { get; set; }

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
        private readonly IUserRepository _userRepository;
        private readonly IPlaceRepository _placeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePlaceCommandHandler(IUserRepository userRepository,
                                         IPlaceRepository placeRepository,
                                         IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _placeRepository = placeRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task<long> HandleCommand(CreatePlaceCommand command, CancellationToken cancellationToken)
        {
            var location = Location.From(command.Latitude, command.Longitude);

            var user = await _userRepository.FindByIdAsync(command.UserId) ?? throw new UserNotFoundException(new { command.UserId });

            var place = Place.Create(user, command.Name, location);
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
