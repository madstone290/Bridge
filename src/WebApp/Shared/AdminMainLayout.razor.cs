using Bridge.WebApp.Services.Identity;
using Microsoft.AspNetCore.Components;

namespace Bridge.WebApp.Shared
{
    public partial class AdminMainLayout
    {
        /// <summary>
        /// 인증 여부
        /// </summary>
        private bool _isAuthenticated;

        /// <summary>
        /// 관리자인가?
        /// </summary>
        private bool _isAdmin;

        /// <summary>
        /// 드로어 열림 상태
        /// </summary>
        private bool _drawerOpen;

        [Inject]
        public IAuthService AuthService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthService.GetAuthStateAsync();
            _isAuthenticated = authState.IsAuthenticated;
            _isAdmin = authState.UserType == Bridge.Shared.Constants.ClaimConstants.Admin;
        }

        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

    }
}
