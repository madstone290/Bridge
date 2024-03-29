using Bridge.Application.Common;
using Bridge.Application.Common.Exceptions.EntityNotFoundExceptions;
using Bridge.Application.Common.Services;
using Bridge.Application.Places.Dtos;
using Bridge.Application.Users;
using Bridge.Domain.Places.Enums;
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
        /// 사용자
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// 장소 아이디
        /// </summary>
        public Guid Id { get; set; }

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
        /// 이미지 변경 여부
        /// </summary>
        public bool ImageChanged { get; set; }

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
        public IEnumerable<OpeningTimeDto> OpeningTimes { get; set; } = Enumerable.Empty<OpeningTimeDto>();
    }

    public class UpdatePlaceCommandHandler : CommandHandler<UpdatePlaceCommand, Unit>
    {
        private readonly IAddressLocationService _addressLocationService;
        private readonly IFileUploadService _fileUploadService;
        private readonly IPlaceRepository _placeRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePlaceCommandHandler(IAddressLocationService addressMapService,
                                         IFileUploadService fileUploadService,
                                         IPlaceRepository placeRepository,
                                         IUnitOfWork unitOfWork,
                                         IUserService userService)
        {
            _addressLocationService = addressMapService;
            _fileUploadService = fileUploadService;
            _placeRepository = placeRepository;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public override async Task<Unit> HandleCommand(UpdatePlaceCommand command, CancellationToken cancellationToken)
        {
            var place = await _placeRepository.FindByIdAsync(command.Id) ?? throw new PlaceNotFoundException(new { command.Id });
            await PermissionChecker.ThrowIfNoPermission(place, command.UserId, _userService);

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

                place.SetDayoff(openingTimeDto.Day, openingTimeDto.IsDayoff);
                place.SetTwentyFourHours(openingTimeDto.Day, openingTimeDto.Is24Hours);
            }

            if (command.ImageChanged)
            {
                if (place.ImagePath != null)
                    _fileUploadService.DeleteFile(place.ImagePath);

                if (command.ImageName != null && command.ImageData != null)
                    place.ImagePath = _fileUploadService.UploadFile("PlaceImages", command.ImageName, command.ImageData);
                else
                    place.ImagePath = null;
            }

            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
