using Bridge.Application.Common;
using Bridge.Application.Common.Services;
using Bridge.Domain.Places.Entities.Places;
using Bridge.Domain.Places.Repos;
using MediatR;

namespace Bridge.Application.Places.Commands
{
    public record CreateRestroomBatchCommand : ICommand<List<string>>
    {
        public IEnumerable<CreateRestroomCommand> Commands { get; set; } = Enumerable.Empty<CreateRestroomCommand>();
    }

    public class CreateRestroomBatchCommandHandler : CommandHandler<CreateRestroomBatchCommand, List<string>>
    {
        private readonly IAddressLocationService _addressLocationService;
        private readonly IPlaceRepository _placeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRestroomBatchCommandHandler(IAddressLocationService addressMapService,
                                            IPlaceRepository placeRepository,
                                            IUnitOfWork unitOfWork)
        {
            _addressLocationService = addressMapService;
            _placeRepository = placeRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task<List<string>> HandleCommand(CreateRestroomBatchCommand batchCommand, CancellationToken cancellationToken)
        {
            var errors = new List<string>();
            var restroomList = new List<Restroom>();
            var taskList = new List<Task>();
            foreach(var command in batchCommand.Commands)
            {
                var task = _addressLocationService.CreateAddressLocationAsync(command.Address.BaseAddress, command.Address.DetailAddress)
                    .ContinueWith(t =>
                    {
                        if (t.Exception != null)
                        {
                            errors.Add($"실패: {command}");
                            return;
                        }

                        try
                        {
                            var addressLocation = t.Result;
                            var restroom = new Restroom(command.Name, addressLocation.Item1, addressLocation.Item2);
                            restroom.SetLastUpdate(command.LastUpdateDateTimeLocal);
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
                            restroomList.Add(restroom);
                        }
                        catch
                        {
                            errors.Add($"실패: {command}");
                        }
                    });

                taskList.Add(task);
            };

            await Task.WhenAll(taskList);

            if(!errors.Any())
            {
                await _placeRepository.AddRangeAsync(restroomList);
                await _unitOfWork.CommitAsync();
            }
            return errors;
        }
    }

}

