using Bridge.Application.Places.Commands;
using Bridge.WebApp.Api.ApiClients.Admin;
using Bridge.WebApp.Pages.Admin.Models;
using Bridge.WebApp.Services;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Bridge.WebApp.Pages.Admin.ViewModels.Implement
{
    public class PlaceFormViewModel : IPlaceFormViewModel
    {
        private readonly Place _place = new();
        private readonly Place.Validator _validator = new();

        private readonly AdminPlaceApiClient _placeApiClient;
        private readonly IApiResultValidationService _validationService;
        private readonly ISnackbar _snackbar;

        public PlaceFormViewModel(AdminPlaceApiClient placeApiClient, IApiResultValidationService validationService, ISnackbar snackbar)
        {
            _placeApiClient = placeApiClient;
            _validationService = validationService;
            _snackbar = snackbar;
        }

        public long PlaceId { get; set; }
        public FormMode FormMode { get; set; }
        public bool IsPlaceValid { get; set; }
        public Place Place => _place;
        public MudDialogInstance MudDialog { get; set; } = null!;
        public FormMode Mode { get; set; }

        private async Task<bool> CreateAsync()
        {
            var command = new CreatePlaceCommand()
            {
                Type = _place.Type,
                Name = _place.Name,
                Address = new Application.Places.Dtos.AddressDto()
                {
                    BaseAddress = _place.BaseAddress,
                    DetailAddress = _place.DetailAddress,
                },
                Categories = _place.Categories.ToList(),
                ContactNumber = _place.ContactNumber,
                ImageName = _place.ImageName,
                ImageData = _place.ImageData,
                OpeningTimes = _place.OpeningTimes.Select(t => new Application.Places.Dtos.OpeningTimeDto()
                {
                    Day = t.Day,
                    Dayoff = t.IsDayoff,
                    TwentyFourHours = t.Is24Hours,
                    BreakEndTime = t.BreakEndTime,
                    BreakStartTime = t.BreakStartTime,
                    OpenTime = t.OpenTime,
                    CloseTime = t.CloseTime,
                }).ToList(),
            };
            var result = await _placeApiClient.CreatePlace(command);
            return _validationService.Validate(result);
        }

        private async Task<bool> UpdateAsync()
        {
            var command = new UpdatePlaceCommand()
            {
                Id = _place.Id,
                Name = _place.Name,
                Address = new Application.Places.Dtos.AddressDto()
                {
                    BaseAddress = _place.BaseAddress,
                    DetailAddress = _place.DetailAddress,
                },
                Categories = _place.Categories.ToList(),
                ContactNumber = _place.ContactNumber,
                ImageChanged = _place.ImageChanged,
                ImageName = _place.ImageName,
                ImageData = _place.ImageData,
                OpeningTimes = _place.OpeningTimes.Select(t => new Application.Places.Dtos.OpeningTimeDto()
                {
                    Day = t.Day,
                    Dayoff = t.IsDayoff,
                    TwentyFourHours = t.Is24Hours,
                    BreakEndTime = t.BreakEndTime,
                    BreakStartTime = t.BreakStartTime,
                    OpenTime = t.OpenTime,
                    CloseTime = t.CloseTime,
                }).ToList(),
            };
            var result = await _placeApiClient.UpdatePlace(command);

            return _validationService.Validate(result);
        }


        public Func<TProperty, Task<IEnumerable<string>>> GetValidation<TProperty>(System.Linq.Expressions.Expression<Func<Place, TProperty>> expression)
        {
            return _validator.PropertyValidation(expression);
        }

        public async Task Initialize()
        {
            if (FormMode == FormMode.Update)
            {
                var result = await _placeApiClient.GetPlaceById(PlaceId);
                if (!_validationService.Validate(result))
                    return;

                var placeDto = result.Data!;
                _place.Id = placeDto.Id;
                _place.Type = placeDto.Type;
                _place.Name = placeDto.Name;
                _place.BaseAddress = placeDto.Address.BaseAddress;
                _place.DetailAddress = placeDto.Address.DetailAddress;
                _place.Categories = placeDto.Categories;
                _place.ContactNumber = placeDto.ContactNumber;
                _place.OpeningTimes = placeDto.OpeningTimes.Select(x => OpeningTime.Create(x));

                if (placeDto.ImagePath != null)
                    _place.ImageUrl = new Uri(_placeApiClient.HttpClient.BaseAddress!, placeDto.ImagePath).ToString();

            }
        }

        public async Task OnCancelClick()
        {
            MudDialog.Cancel();
            await Task.CompletedTask;
        }

        public async Task OnSaveClick()
        {
            if (!IsPlaceValid)
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

            if (success)
                MudDialog.Close(PlaceId);

        }

        public async Task OnImageFileChange(InputFileChangeEventArgs args)
        {
            var file = args.File;
            var sizeLimit = 50000;
            if (sizeLimit < file.Size)
            {
                _snackbar.Add("50Kb가 넘는 이미지는 사용할 수 없습니다");
                return;
            }

            var format = file.ContentType;
            var buffer = new byte[file.Size];
            using var stream = file.OpenReadStream(file.Size);
            await stream.ReadAsync(buffer);

            var base64 = Convert.ToBase64String(buffer);
            _place.ImageUrl = $"data:{format};base64,{base64}";
            _place.ImageData = buffer;
            _place.ImageName = file.Name;
            _place.ImageChanged = true;
        }
        
    }
}
