using System.ComponentModel.DataAnnotations;

namespace Bridge.Shared.ApiContract.Dtos.Identity
{
    public class RegisterDto
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;
    }
}
