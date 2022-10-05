using Bridge.Api.Controllers.Identity.Dtos;
using Bridge.Infrastructure.Identity;
using Bridge.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bridge.Api.Controllers.Identity
{
    public class UsersController : ApiController
    {
        private readonly UserService _userService;
        private readonly string _welcomeHtmlPath;

        public UsersController(UserService userService, IWebHostEnvironment hostEnvironment)
        {
            _userService = userService;
            _welcomeHtmlPath = Path.Combine(hostEnvironment.WebRootPath, "welcome.html");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(ApiRoutes.Users.Register)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAsync([FromBody] UserDto userDto)
        {
            var userId = await _userService.RegisterAsync(userDto.Email, userDto.Password, userDto.UserName);
            return Ok(userId);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(ApiRoutes.Users.SendVerificationEmail)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SendVerificationEmailAsync([FromBody] VerificationDto verificationDto)
        {
            var callbackUri = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.ToString() + ApiRoutes.Users.VerifyEmail;
            await _userService.SendVerificationEmailAsync(verificationDto.Email, verificationDto.RedirectUri, callbackUri.ToString());
            return Ok(callbackUri);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route(ApiRoutes.Users.VerifyEmail)]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public async Task<IActionResult> VerifyEmailAsync([FromQuery] string email, [FromQuery] string token, [FromQuery] string? redirectUri)
        {
            await _userService.VerifyEmailAsync(email, token);
            if (string.IsNullOrEmpty(redirectUri))
            {
                var html = System.IO.File.ReadAllText(_welcomeHtmlPath);
                return Content(html, "text/html");
            }
            else
            {
                return Redirect(redirectUri);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(ApiRoutes.Users.Login)]
        [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            var result = await _userService.LoginAsync(loginDto.Email, loginDto.Password);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(ApiRoutes.Users.Refresh)]
        [ProducesResponseType(typeof(RefreshResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshAsync([FromBody] RefreshDto refreshDto)
        {
            var result = await _userService.RefreshAsync(refreshDto.Email, refreshDto.RefreshToken);
            return Ok(result);
        }

    }
}
