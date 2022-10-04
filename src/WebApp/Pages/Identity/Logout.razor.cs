using Bridge.WebApp.Services.Identity;
using Microsoft.AspNetCore.Components;

namespace Bridge.WebApp.Pages.Identity
{
    public partial class Logout
    { 
        /// <summary>
        /// 로그아웃 완료 후 리디렉트할 URI
        /// </summary>
        [Parameter]
        [SupplyParameterFromQuery]
        public string? RedirectUri { get; set; }

        [Inject]
        public IAuthService AuthService { get; set; } = null!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await AuthService.LogoutAsync();

                NavManager.NavigateTo(RedirectUri ?? PageRoutes.Home, true);
            }
        }
      
    }
}
