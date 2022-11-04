using Bridge.Application.Places.Commands;
using Bridge.Domain.Places.Enums;
using Bridge.WebApp.Api.ApiClients.Admin;
using Bridge.WebApp.Pages.Admin.Models;
using Bridge.WebApp.Services;
using MudBlazor;
using System.Linq.Expressions;

namespace Bridge.WebApp.Pages.Admin.ViewModels.Implement
{
    public class RestroomFormViewModel : IRestroomFormViewModel
    {
        private readonly Place.Validator _validator = new();

        private readonly AdminRestroomApiClient _restroomApiClient;
        private readonly IApiResultValidationService _validationService;

        public RestroomFormViewModel(AdminRestroomApiClient restroomApiClient, IApiResultValidationService validationService)
        {
            _restroomApiClient = restroomApiClient;
            _validationService = validationService;
        }

        public MudDialogInstance MudDialog { get; set; } = null!;
        public Guid RestroomId { get; set; }
        public Restroom Restroom { get; private set; } = new();
        public FormMode FormMode { get; set; }
        public bool IsRestroomValid { get; set; }

        private async Task<bool> CreateAsync()
        {
            var command = new CreateRestroomCommand()
            {
                Name = Restroom.Name,
                Address = new Application.Places.Dtos.AddressDto()
                {
                    BaseAddress = Restroom.BaseAddress,
                    DetailAddress = Restroom.DetailAddress,
                },
                IsUnisex = Restroom.IsUnisex,
                DiaperTableLocation = Restroom.DiaperTableLocation,
                MaleToilet = Restroom.MaleToilet,
                MaleUrinal = Restroom.MaleUrinal,
                MaleDisabledToilet = Restroom.MaleDisabledToilet,
                MaleDisabledUrinal = Restroom.MaleDisabledUrinal,
                MaleKidToilet = Restroom.MaleKidToilet,
                MaleKidUrinal = Restroom.MaleKidUrinal,
                FemaleToilet = Restroom.FemaleToilet,
                FemaleDisabledToilet = Restroom.FemaleDisabledToilet,
                FemaleKidToilet = Restroom.FemaleKidToilet,
                OpeningTimes = Restroom.OpeningTimes.Select(t => new Application.Places.Dtos.OpeningTimeDto()
                {
                    Day = t.Day,
                    IsDayoff = t.IsDayoff,
                    Is24Hours = t.Is24Hours,
                    BreakEndTime = t.BreakEndTime,
                    BreakStartTime = t.BreakStartTime,
                    OpenTime = t.OpenTime,
                    CloseTime = t.CloseTime,
                }).ToList(),
            };
            var result = await _restroomApiClient.CreateRestroom(command);
            var success = _validationService.Validate(result);

            if (success)
            {
                RestroomId = result.Data;
                Restroom.Id = result.Data;
            }
            return success;
        }

        private async Task<bool> UpdateAsync()
        {
            var command = new UpdateRestroomCommand()
            {
                Id = Restroom.Id,
                Name = Restroom.Name,
                Address = new Application.Places.Dtos.AddressDto()
                {
                    BaseAddress = Restroom.BaseAddress,
                    DetailAddress = Restroom.DetailAddress,
                },
                IsUnisex = Restroom.IsUnisex,
                DiaperTableLocation = Restroom.DiaperTableLocation,
                MaleToilet = Restroom.MaleToilet,
                MaleUrinal = Restroom.MaleUrinal,
                MaleDisabledToilet = Restroom.MaleDisabledToilet,
                MaleDisabledUrinal = Restroom.MaleDisabledUrinal,
                MaleKidToilet = Restroom.MaleKidToilet,
                MaleKidUrinal = Restroom.MaleKidUrinal,
                FemaleToilet = Restroom.FemaleToilet,
                FemaleDisabledToilet = Restroom.FemaleDisabledToilet,
                FemaleKidToilet = Restroom.FemaleKidToilet,
                OpeningTimes = Restroom.OpeningTimes.Select(t => new Application.Places.Dtos.OpeningTimeDto()
                {
                    Day = t.Day,
                    IsDayoff = t.IsDayoff,
                    Is24Hours = t.Is24Hours,
                    BreakEndTime = t.BreakEndTime,
                    BreakStartTime = t.BreakStartTime,
                    OpenTime = t.OpenTime,
                    CloseTime = t.CloseTime,
                }).ToList(),
            };
            var result = await _restroomApiClient.UpdateRestroom(command);
            return _validationService.Validate(result);
        }

        public string GetDiaperTableLocationText(DiaperTableLocation? location)
        {
            if (location == DiaperTableLocation.MaleToilet)
                return "남자 화장실";
            else if (location == DiaperTableLocation.FemaleToilet)
                return "여자 화장실";
            else
                return "알 수 없음";
        }

        public Func<TProperty, Task<IEnumerable<string>>> GetValidation<TProperty>(Expression<Func<Place, TProperty>> expression)
        {
            return _validator.PropertyValidation(expression);
        }

        public async Task Initialize()
        {
            if (FormMode == FormMode.Update)
            {
                var result = await _restroomApiClient.GetRestroomById(RestroomId);
                if (!_validationService.Validate(result))
                    return;

                var restroomDto = result.Data!;
                Restroom.Id = restroomDto.Id;
                Restroom.Name = restroomDto.Name;
                Restroom.BaseAddress = restroomDto.Address.BaseAddress;
                Restroom.DetailAddress = restroomDto.Address.DetailAddress;
                Restroom.IsUnisex = restroomDto.IsUnisex;
                Restroom.DiaperTableLocation = restroomDto.DiaperTableLocation;
                Restroom.MaleToilet = restroomDto.MaleToilet;
                Restroom.MaleUrinal = restroomDto.MaleUrinal;
                Restroom.MaleDisabledToilet = restroomDto.MaleDisabledToilet;
                Restroom.MaleDisabledUrinal = restroomDto.MaleDisabledUrinal;
                Restroom.MaleKidToilet = restroomDto.MaleKidToilet;
                Restroom.MaleKidUrinal = restroomDto.MaleKidUrinal;
                Restroom.FemaleToilet = restroomDto.FemaleToilet;
                Restroom.FemaleDisabledToilet = restroomDto.FemaleDisabledToilet;
                Restroom.FemaleKidToilet = restroomDto.FemaleKidToilet;
                Restroom.OpeningTimes = restroomDto.OpeningTimes.Select(x => OpeningTime.Create(x)).ToList();
            }
        }

        public Task OnCancelClick()
        {
            MudDialog.Cancel();
            return Task.CompletedTask;
        }

        public async Task OnSaveClick()
        {
            if (!IsRestroomValid)
                return;

            bool success;
            if (FormMode == FormMode.Create)
            {
                success = await CreateAsync();
            }
            else
            {
                success = await UpdateAsync();
            }

            if(success)
                MudDialog.Close(Restroom.Id);
        }

    }
}
