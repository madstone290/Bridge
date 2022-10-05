using Bridge.WebApp.Pages.Identity.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Bridge.WebApp.Api.ApiClients.Identity;
using Bridge.Api.Controllers.Identity.Dtos;
using Bridge.Shared.Extensions;

namespace Bridge.WebApp.Pages.Identity
{
    public partial class Register
    {
        private readonly RegisterModel _registerModel = new();
        private readonly RegisterModel.Validator _validator = new();

        private MudForm? _form;

        /// <summary>
        /// 에러 메시지
        /// </summary>
        [Parameter]
        [SupplyParameterFromQuery]
        public string? Error { get; set; }

        [Inject]
        public UserApiClient UserApiClient { get; set; } = null!;

        private async Task Register_Click()
        {
            if (_form == null)
                return;

            Error = string.Empty;

            await _form.Validate();

            if (_form.IsValid)
            {
                var result = await UserApiClient.RegisterAsync(new RegisterDto()
                {
                    Email = _registerModel.Email,
                    Password = _registerModel.Password,
                    UserName = _registerModel.UserName
                });

                if (result.Success)
                {
                    NavManager.NavigateTo(PageRoutes.Identity.SendVerificationEmail
                        .AddQueryParam(nameof(SendVerificationEmail.Email), _registerModel.Email));
                }
                else
                {
                    Error = result.ErrorMessage;
                }

            }
        }
       
    }
}
