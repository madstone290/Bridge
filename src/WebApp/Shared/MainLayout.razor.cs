using Bridge.WebApp.Services.Identity;
using Microsoft.AspNetCore.Components;

namespace Bridge.WebApp.Shared
{
    public partial class MainLayout
    {
        /// <summary>
        /// 인증 여부
        /// </summary>
        private bool _isAuthenticated;

        [Inject]
        public IAuthService AuthService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthService.GetAuthStateAsync();
            _isAuthenticated = authState.IsAuthenticated;
        }

    }
}
