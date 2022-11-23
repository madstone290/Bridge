using Bridge.Application.Places.Commands;
using Bridge.WebApp.Api.ApiClients;
using Bridge.WebApp.Pages.Common.Models;
using Bridge.WebApp.Services;
using Bridge.WebApp.Services.ReverseGeocode;
using MudBlazor;

namespace Bridge.WebApp.Pages.Home.ViewModels.Implement
{
    public class PlaceAddViewModel : IPlaceAddViewModel
    {
        private readonly Place.Validator _validator = new();
        private readonly PlaceApiClient _placeApiClient;
        private readonly IResultValidationService _validationService;

        public PlaceAddViewModel(PlaceApiClient placeApiClient, IReverseGeocodeService reverseGeocodeService, IResultValidationService validationService)
        {
            _placeApiClient = placeApiClient;
            _validationService = validationService;
        }

        public bool IsPlaceValid { get; set; }
        public Place Place { get; set; } = new();
        public LatLon PlaceLocation { get; set; } = new(0, 0);
        public MudDialogInstance MudDialog { get; set; } = null!;

        public Func<TProperty, Task<IEnumerable<string>>> GetValidation<TProperty>(System.Linq.Expressions.Expression<Func<Place, TProperty>> expression)
        {
            return _validator.PropertyValidation(expression);
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

            var command = new CreatePlaceCommand()
            {
                Type = Place.Type,
                Name = Place.Name,
                Address = new Application.Places.Dtos.AddressDto()
                {
                    BaseAddress = Place.BaseAddress,
                    DetailAddress = Place.DetailAddress,
                },
                Categories = Place.Categories.ToList(),
                ContactNumber = Place.ContactNumber,
                OpeningTimes = Place.OpeningTimes.Select(t => new Application.Places.Dtos.OpeningTimeDto()
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

            // 저장
            var result = await _placeApiClient.AddPlace(command);

            if (_validationService.Validate(result))
                MudDialog.Close(result.Data);

        }

    }
}
