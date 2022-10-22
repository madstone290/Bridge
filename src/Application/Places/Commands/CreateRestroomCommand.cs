using Bridge.Application.Common;
using Bridge.Application.Common.Services;
using Bridge.Application.Places.Dtos;
using Bridge.Domain.Places.Entities.Places;
using Bridge.Domain.Places.Repos;

namespace Bridge.Application.Places.Commands
{
    public record CreateRestroomCommand : ICommand<long>
    {
        /// <summary>
        /// 화장실명
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 주소
        /// </summary>
        public AddressDto Address { get; set; } = AddressDto.Empty;

        /// <summary>
        /// 최근 업데이트 일시
        /// </summary>
        public DateTime? LastUpdateDateTimeLocal { get; set; }

        /// <summary>
        /// 남녀공용여부
        /// </summary>
        public bool IsUnisex { get; set; }

        /// <summary>
        /// 기저귀 교환대 여부
        /// </summary>
        public bool HasDiaperTable { get; set; }

        /// <summary>
        /// 기저귀 교환대 위치
        /// </summary>
        public DiaperTableLocation? DiaperTableLocation { get; set; }

        public int? MaleToilet { get; set; }

        public int? MaleUrinal { get; set; }

        public int? MaleDisabledToilet { get; set; }

        public int? MaleDisabledUrinal { get; set; }

        public int? MaleKidToilet { get; set; }

        public int? MaleKidUrinal { get; set; }

        public int? FemaleToilet { get; set; }

        public int? FemaleKidToilet { get; set; }

        public int? FemaleDisabledToilet { get; set; }

        /// <summary>
        /// 영업시간
        /// </summary>
        public List<OpeningTimeDto> OpeningTimes { get; set; } = new();
    }

    public class CreateRestroomCommandHandler : CommandHandler<CreateRestroomCommand, long>
    {
        private readonly IAddressLocationService _addressLocationService;
        private readonly IPlaceRepository _placeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRestroomCommandHandler(IAddressLocationService addressMapService,
                                            IPlaceRepository placeRepository,
                                            IUnitOfWork unitOfWork)
        {
            _addressLocationService = addressMapService;
            _placeRepository = placeRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task<long> HandleCommand(CreateRestroomCommand command, CancellationToken cancellationToken)
        {
            var addressLocation = await _addressLocationService.CreateAddressLocationAsync(command.Address.BaseAddress, command.Address.DetailAddress);
            var restroom = new Restroom(command.Name, addressLocation.Item1, addressLocation.Item2);
            restroom.SetLastUpdate(command.LastUpdateDateTimeLocal);
            restroom.UpdateRestroom(command.IsUnisex, command.HasDiaperTable, command.DiaperTableLocation,
                                 command.MaleToilet, command.MaleUrinal, command.MaleDisabledToilet,
                                 command.MaleDisabledUrinal, command.MaleKidToilet, command.MaleKidUrinal,
                                 command.FemaleToilet, command.FemaleDisabledToilet, command.FemaleKidToilet);

            foreach (var openingTimeDto in command.OpeningTimes)
            {
                if (openingTimeDto.OpenTime.HasValue && openingTimeDto.CloseTime.HasValue)
                    restroom.SetOpenCloseTime(openingTimeDto.Day, openingTimeDto.OpenTime.Value, openingTimeDto.CloseTime.Value);

                if (openingTimeDto.BreakStartTime.HasValue && openingTimeDto.BreakEndTime.HasValue)
                    restroom.SetBreakTime(openingTimeDto.Day, openingTimeDto.BreakStartTime.Value, openingTimeDto.BreakEndTime.Value);

                restroom.SetDayoff(openingTimeDto.Day, openingTimeDto.Dayoff);
                restroom.SetTwentyFourHours(openingTimeDto.Day, openingTimeDto.TwentyFourHours);
            }

            await _placeRepository.AddAsync(restroom);
            await _unitOfWork.CommitAsync();
            return restroom.Id;
        }
    }
}
