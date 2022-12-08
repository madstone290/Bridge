using Bridge.Application.Common;
using Bridge.Application.Common.Services;
using Bridge.Application.Places.Dtos;
using Bridge.Domain.Places.Entities;
using Bridge.Domain.Places.Enums;
using Bridge.Domain.Places.Repos;

namespace Bridge.Application.Places.Commands
{
    /// <summary>
    /// 장소를 생성한다
    /// </summary>
    public class CreatePlaceCommand : ICommand<Guid>
    {
        /// <summary>
        /// 사용자
        /// </summary>
        public string UserId { get; set; } = string.Empty;

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
        /// 이미지 이름
        /// </summary>
        public string? ImageName { get; set; }

        /// <summary>
        /// 이미지 데이터
        /// </summary>
        public byte[]? ImageData { get; set; }

        /// <summary>
        /// 영업시간
        /// </summary>
        public List<OpeningTimeDto> OpeningTimes { get; set; } = new();
    }

    public class CreatePlaceCommandHandler : CommandHandler<CreatePlaceCommand, Guid>
    {
        private readonly IAddressLocationService _addressLocationService;
        private readonly IFileUploadService _fileUploadService;
        private readonly IPlaceRepository _placeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePlaceCommandHandler(IAddressLocationService addressMapService,
                                         IFileUploadService fileUploadService,
                                         IPlaceRepository placeRepository,
                                         IUnitOfWork unitOfWork)
        {
            _addressLocationService = addressMapService;
            _fileUploadService = fileUploadService;
            _placeRepository = placeRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task<Guid> HandleCommand(CreatePlaceCommand command, CancellationToken cancellationToken)
        {
            var addressLocation = await _addressLocationService.CreateAddressLocationAsync(command.Address.BaseAddress, command.Address.DetailAddress);
            var place = new Place(command.UserId, command.Type, command.Name, addressLocation.Item1, addressLocation.Item2);
            place.SetContactNumber(command.ContactNumber);

            foreach (var category in command.Categories)
                place.AddCategory(category);

            foreach (var openingTimeDto in command.OpeningTimes)
            {
                if (openingTimeDto.OpenTime.HasValue && openingTimeDto.CloseTime.HasValue)
                    place.SetOpenCloseTime(openingTimeDto.Day, openingTimeDto.OpenTime.Value, openingTimeDto.CloseTime.Value);

                if (openingTimeDto.BreakStartTime.HasValue && openingTimeDto.BreakEndTime.HasValue)
                    place.SetBreakTime(openingTimeDto.Day, openingTimeDto.BreakStartTime.Value, openingTimeDto.BreakEndTime.Value);

                place.SetDayoff(openingTimeDto.Day, openingTimeDto.IsDayoff);
                place.SetTwentyFourHours(openingTimeDto.Day, openingTimeDto.Is24Hours);
            }

            if(command.ImageName != null && command.ImageData != null)
            {
                place.ImagePath = _fileUploadService.UploadFile("PlaceImages", command.ImageName, command.ImageData);
            }

            await _placeRepository.AddAsync(place);

            await _unitOfWork.CommitAsync();
            return place.Id;
        }
    }
}
