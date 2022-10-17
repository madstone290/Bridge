using Bridge.WebApp.Api.ApiClients.Admin;
using Bridge.WebApp.Pages.Admin.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bridge.WebApp.Pages.Admin
{
    public partial class PlaceView
    {
        private MudForm? _baseInfoForm;
        
        /// <summary>
        /// 기본정보 유효성 상태
        /// </summary>
        private bool _baseInfoFormValid;

        /// <summary>
        /// 기본정보 읽기전용 상태
        /// </summary>
        private bool _baseInfoReadOnly = true;

        private MudForm? _openingTimeForm;

        /// <summary>
        /// 영업시간 유효성 상태
        /// </summary>
        private bool _openingTimeFormValid;

        /// <summary>
        /// 영업시간 읽기전용 상태
        /// </summary>
        private bool _openingTimeReadOnly = true;


        private readonly PlaceFormModel _place = new();
        private readonly PlaceFormModel _placeBackup = new();
        private readonly PlaceFormModel.Validator _validator = new();
        private readonly List<ProductModel> _products = new();

        /// <summary>
        /// 제품 검색어
        /// </summary>
        private string _searchString = string.Empty;

        /// <summary>
        /// 장소 아이디
        /// </summary>
        [Parameter]
        public long PlaceId { get; set; }

        [Inject]
        public AdminPlaceApiClient PlaceApiClient { get; set; } = null!;


        protected override async Task OnInitializedAsync()
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

            PlaceFormModel.Copy(_place, _placeBackup);
        }

        private void EditBaseInfo_Click()
        {
            _baseInfoReadOnly = false;
        }

        private async void SaveBaseInfo_Click()
        {
            var result = await PlaceApiClient.UpdatePlaceBaseInfo(new Application.Places.Commands.UpdatePlaceBaseInfoCommand()
            {
                Id = _place.Id,
                Address = new Application.Places.Dtos.AddressDto()
                {
                    BaseAddress = _place.BaseAddress,
                    DetailAddress = _place.DetailAddress
                },
                Name = _place.Name,
                Categories = _place.Categories,
                ContactNumber  =_place.ContactNumber,
            });

            if (!ValidationService.Validate(result))
            {
                PlaceFormModel.Copy(_placeBackup, _place);
                return;
            }

            PlaceFormModel.Copy(_place, _placeBackup);
            _baseInfoReadOnly = true;
            StateHasChanged();
        }

        private void CancelBaseInfo_Click()
        {
            PlaceFormModel.Copy(_placeBackup, _place);
            _baseInfoReadOnly = true;
            StateHasChanged();
        }

        private void EditOpeningTime_Click()
        {
            _openingTimeReadOnly = false;
            StateHasChanged();
        }

        private async void SaveOpeningTime_Click()
        {
            var result = await PlaceApiClient.UpdatePlaceOpeningTimes(new Application.Places.Commands.UpdatePlaceOpeningTimesCommand()
            {
                Id = _place.Id,
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
            });

            if (!ValidationService.Validate(result))
            {
                PlaceFormModel.Copy(_placeBackup, _place);
                return;
            }

            PlaceFormModel.Copy(_place, _placeBackup);
            _openingTimeReadOnly = true;
            StateHasChanged();
        }

        private void CancelOpeningTime_Click()
        {
            PlaceFormModel.Copy(_placeBackup, _place);
            _openingTimeReadOnly = true;
            StateHasChanged();
        }
    }
}
