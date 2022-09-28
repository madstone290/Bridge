using System.ComponentModel.DataAnnotations;

namespace Bridge.Api.Controllers.Identity.Dtos
{
    public class LoginDto
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
