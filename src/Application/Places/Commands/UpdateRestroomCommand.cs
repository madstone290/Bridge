using Bridge.Application.Common;
using Bridge.Application.Common.Exceptions.EntityNotFoundExceptions;
using Bridge.Application.Common.Services;
using Bridge.Application.Places.Dtos;
using Bridge.Domain.Places.Entities.Places;
using Bridge.Domain.Places.Repos;
using MediatR;

namespace Bridge.Application.Places.Commands
{
    public class UpdateRestroomCommand : ICommand<Unit>
    {
        /// <summary>
        /// 장소 아이디
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 화장실명
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 주소
        /// </summary>
        public AddressDto Address { get; set; } = AddressDto.Empty;

        /// <summary>
        /// 남녀공용여부
        /// </summary>
        public bool IsUnisex { get; set; }

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

    public class UpdateRestroomCommandHandler : CommandHandler<UpdateRestroomCommand, Unit>
    {
        private readonly IAddressLocationService _addressLocationService;
        private readonly IPlaceRepository _placeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRestroomCommandHandler(IAddressLocationService addressMapService,
                                            IPlaceRepository placeRepository,
                                            IUnitOfWork unitOfWork)
        {
            _addressLocationService = addressMapService;
            _placeRepository = placeRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task<Unit> HandleCommand(UpdateRestroomCommand command, CancellationToken cancellationToken)
        {
            var restroom = (Restroom?)(await _placeRepository.FindByIdAsync(command.Id)) ?? throw new PlaceNotFoundException( new { command.Id });
            
            restroom.SetName(command.Name);
            var addressLocation = await _addressLocationService.CreateAddressLocationAsync(command.Address.BaseAddress, command.Address.DetailAddress);
            restroom.SetAddressLocation(addressLocation.Item1, addressLocation.Item2);
            restroom.UpdateRestroom(command.IsUnisex, command.DiaperTableLocation,
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

            await _unitOfWork.CommitAsync();
            return Unit.Value;
        }
    }
}
