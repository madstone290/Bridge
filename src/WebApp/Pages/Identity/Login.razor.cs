using Bridge.WebApp.Constants;
using Bridge.WebApp.Pages.Identity.Models;
using Bridge.WebApp.Services.Identity;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Bridge.WebApp.Services;

namespace Bridge.WebApp.Pages.Identity
{
    public partial class Login
    {
        private readonly LoginModel _loginModel = new();
        private readonly LoginModel.Validator _validator = new();

        private MudForm? _form;
        private MudTextField<string>? _passwordField;
        private MudTextField<string>? _emailField;

        /// <summary>
        /// 에러 메시지
        /// </summary>
        [Parameter]
        [SupplyParameterFromQuery]
        public string? Error { get; set; }

        /// <summary>
        /// 로그인 완료 후 리디렉트할 URI
        /// </summary>
        [Parameter]
        [SupplyParameterFromQuery]
        public string? RedirectUri { get; set; }

        [Inject]
        public ILocalStorageService LocalStorageService { get; set; } = null!;

        [Inject]
        public IAuthService AuthService { get; set; } = null!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadOptionsAsync();
                StateHasChanged();

                if (_loginModel.RememberMe)
                    await _passwordField!.FocusAsync();
                else
                    await _emailField!.FocusAsync();
            }
        }

        private async Task Password_OnKeyUp(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await RequestLoginAsync();
            }
        }


        /// <summary>
        /// 로그인 옵션을 불러온다.
        /// </summary>
        /// <returns></returns>
        private async Task LoadOptionsAsync()
        {
            _loginModel.RememberMe = await LocalStorageService.TryGetItemAsync<bool>(LocalStorageKeyConstants.RememberMe);

            if (_loginModel.RememberMe)
            {
                _loginModel.Email = await LocalStorageService.TryGetItemAsync<string>(LocalStorageKeyConstants.Email) ?? string.Empty;
            }
        }

        /// <summary>
        /// 로그인 옵션을 저장한다.
        /// </summary>
        /// <returns></returns>
        private async Task SaveOptionsAsync()
        {
            await LocalStorageService.SetItemAsync(LocalStorageKeyConstants.RememberMe, _loginModel.RememberMe);
            
            if (_loginModel.RememberMe)
                await LocalStorageService.SetItemAsync(LocalStorageKeyConstants.Email, _loginModel.Email);
            else
                await LocalStorageService.TryRemoveItemAsync(LocalStorageKeyConstants.Email);
        }

        private async Task RequestLoginAsync()
        {
            if (_form == null)
                return;

            Error = string.Empty;

            await _form.Validate();

            if (_form.IsValid)
            {
                // 설정 저장하기
                await SaveOptionsAsync();

                var authResult = await AuthService.LoginAsync(_loginModel.Email, _loginModel.Password);
                if (authResult.Success)
                    NavManager.NavigateTo(RedirectUri ?? PageRoutes.Home, true);
                else
                    Error = authResult.Error;

            }
        }
    }
}
