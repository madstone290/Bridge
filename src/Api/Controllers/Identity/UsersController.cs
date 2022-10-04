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

        public UsersController(UserService userService)
        {
            _userService = userService;
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
        public async Task<IActionResult> VerifyEmailAsync([FromQuery] string email, [FromQuery] string token, [FromQuery] string redirectUri)
        {
            await _userService.VerifyEmailAsync(email, token);
            return Redirect(redirectUri);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(ApiRoutes.Users.Login)]
        [ProducesResponseType(typeof(RefreshResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            var tokenResult = await _userService.LoginAsync(loginDto.Email, loginDto.Password);
            return Ok(tokenResult);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(ApiRoutes.Users.Refresh)]
        [ProducesResponseType(typeof(RefreshResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshAsync([FromBody] RefreshDto refreshDto)
        {
            var tokenResult = await _userService.RefreshAsync(refreshDto.Email, refreshDto.RefreshToken);
            return Ok(tokenResult);
        }

    }
}
