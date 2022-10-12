using Bridge.Application.Places.Commands;
using Bridge.WebApp.Api.ApiClients;
using Bridge.WebApp.Extensions;
using Bridge.WebApp.Pages.Admin.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bridge.WebApp.Pages.Admin
{
    public partial class PlaceForm
    {
        private MudForm? _form;
        private bool _isFormValid;
        private readonly PlaceFormModel _place = new();
        private readonly PlaceFormModel.Validator _validator = new();

        /// <summary>
        /// 폼 모드
        /// </summary>
        [Parameter] public FormMode FormMode { get; set; }

        /// <summary>
        /// 장소 아이디
        /// </summary>
        [Parameter] public long PlaceId { get; set; } 

        [Inject] public PlaceApiClient PlaceApiClient { get; set; } = null!;

        void Cancel_Click()
        {
            NavManager.NavigateTo(PageRoutes.Admin.PlaceList);
        }

        async Task Save_Click()
        {
            if (_form == null)
                return;

            await _form.Validate();

            if (_isFormValid)
            {

                if (FormMode == FormMode.Create)
                {
                    var command = new CreatePlaceCommand()
                    {
                        Type = _place.Type,
                        Name = _place.Name,
                        Address = new Application.Places.Dtos.AddressDto()
                        {
                            BaseAddress = _place.Address,
                            Details = _place.Address,
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

                    if (Snackbar.CheckSuccess(result))
                        NavManager.NavigateTo(PageRoutes.Admin.PlaceList);
                }
                else
                {
                    // 수정

                    // 성공 -> Home 이동

                    // 실패 -> 메시지 출력
                }

            }
        }
    }
}

