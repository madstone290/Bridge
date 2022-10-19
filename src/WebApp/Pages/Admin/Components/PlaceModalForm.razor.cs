using Bridge.Application.Places.Commands;
using Bridge.WebApp.Api.ApiClients.Admin;
using Bridge.WebApp.Pages.Admin.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bridge.WebApp.Pages.Admin.Components
{
    public partial class PlaceModalForm
    {
        private MudForm? _form;
        private readonly PlaceFormModel _place = new();
        private readonly PlaceFormModel.Validator _validator = new();

        [CascadingParameter]
        public MudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// 장소 아이디
        /// </summary>
        [Parameter]
        public long PlaceId { get; set; }

        /// <summary>
        /// 폼 모드
        /// </summary>
        [Parameter]
        public FormMode FormMode { get; set; }


        [Inject]
        public AdminPlaceApiClient PlaceApiClient { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            if (FormMode == FormMode.Update)
            {
                var result = await PlaceApiClient.GetPlaceById(PlaceId);
                if (!ValidationService.Validate(result))
                    return;

                var placeDto = result.Data!;
                _place.Id = placeDto.Id;
                _place.Type = placeDto.Type;
                _place.Name = placeDto.Name;
                _place.BaseAddress = placeDto.Address.BaseAddress;
                _place.DetailAddress = placeDto.Address.DetailAddress;
                _place.Categories = placeDto.Categories;
                _place.ContactNumber = placeDto.ContactNumber;
                _place.OpeningTimes = placeDto.OpeningTimes.Select(x => OpeningTimeFormModel.Create(x));
            }
        }

        private void Cancel_Click()
        {
            MudDialog.Cancel();
        }

        private async void Save_Click()
        {
            if (_form == null)
                return;

            await _form.Validate();

            if (_form.IsValid)
            {
                if (FormMode == FormMode.Create)
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
                        OpeningTimes = _place.OpeningTimes.Select(t => new Application.Places.Dtos.OpeningTimeDto()
                        {
                            Day = t.Day,
                            Dayoff = t.Dayoff,
                            TwentyFourHours = t.TwentyFourHours,
                            BreakEndTime = t.BreakEndTime,
                            BreakStartTime = t.BreakStartTime,
                            OpenTime = t.OpenTime,
                            CloseTime = t.CloseTime,
                        }).ToList(),
                    };
                    var result = await PlaceApiClient.CreatePlace(command);

                    if (ValidationService.Validate(result))
                        MudDialog.Close(result.Data);
                }
                else
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
                        OpeningTimes = _place.OpeningTimes.Select(t => new Application.Places.Dtos.OpeningTimeDto()
                        {
                            Day = t.Day,
                            Dayoff = t.Dayoff,
                            TwentyFourHours = t.TwentyFourHours,
                            BreakEndTime = t.BreakEndTime,
                            BreakStartTime = t.BreakStartTime,
                            OpenTime = t.OpenTime,
                            CloseTime = t.CloseTime,
                        }).ToList(),
                    };
                    var result = await PlaceApiClient.UpdatePlace(command);

                    if (ValidationService.Validate(result))
                        MudDialog.Close(_place.Id);
                }

            }
        }
    }
}
