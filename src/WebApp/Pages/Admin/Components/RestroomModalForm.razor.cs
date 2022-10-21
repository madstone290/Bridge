using Bridge.Domain.Places.Entities.Places;
using Bridge.WebApp.Api.ApiClients.Admin;
using Bridge.WebApp.Pages.Admin.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bridge.WebApp.Pages.Admin.Components
{
    public partial class RestroomModalForm
    {
        private MudForm? _form;
        private readonly RestroomFormModel _model = new();
        private readonly PlaceFormModel.Validator _validator = new();

        public static string DiaperTableLocationText(DiaperTableLocation? location)
        {
            if (location == DiaperTableLocation.MaleToilet)
                return "남자 화장실";
            else if (location == DiaperTableLocation.FemaleToilet)
                return "여자 화장실";
            else
                return "알 수 없음";
        }

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
        public AdminRestroomApiClient RestroomApiClient { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            _model.RestroomApiClient = RestroomApiClient;

            if (FormMode == FormMode.Update)
            {
                var result = await _model.LoadRestroomAsync(PlaceId);
                ValidationService.Validate(result);
            }
        }

        private void Cancel_Click()
        {
            MudDialog.Cancel();
        }

        private async void Save_Click()
        {
            await _form!.Validate();

            if (!_form.IsValid)
                return;

            if (FormMode == FormMode.Create)
            {
                var result = await _model.CreateNewRestroomAsync();

                if (ValidationService.Validate(result))
                    MudDialog.Close(_model.Id);
            }
            else
            {
                var result = await _model.UpdateRestroomAsync();

                if (ValidationService.Validate(result))
                    MudDialog.Close(_model.Id);
            }

        }

    }
}
