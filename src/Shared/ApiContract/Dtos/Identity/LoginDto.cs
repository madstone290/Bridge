using System.ComponentModel.DataAnnotations;

namespace Bridge.Shared.ApiContract.Dtos.Identity
{
    public class LoginDto
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
