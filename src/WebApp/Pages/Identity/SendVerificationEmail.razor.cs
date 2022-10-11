using Bridge.Api.Controllers.Identity.Dtos;
using Bridge.WebApp.Api.ApiClients.Identity;
using Microsoft.AspNetCore.Components;

namespace Bridge.WebApp.Pages.Identity
{
    public partial class SendVerificationEmail
    {
        /// <summary>
        /// 에러메시지
        /// </summary>
        private string? _error;

        /// <summary>
        /// 수신 이메일
        /// </summary>
        [Parameter]
        [SupplyParameterFromQuery]
        public string? Email { get; set; }

        [Inject]
        public UserApiClient UserApiClient { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await SendEmail_Click();
        }

        private async Task SendEmail_Click()
        {
            if (Email == null)
            {
                _error = "이메일이 없습니다";
                return;
            }

            var result = await UserApiClient.SendVerificationEmailAsync(new VerificationDto()
            {
                Email = Email,
            });

            if (!result.Success)
                _error = result.Error;
        }
    }
}
