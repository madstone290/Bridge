using System.ComponentModel.DataAnnotations;

namespace Bridge.Api.Controllers.Identity.Dtos
{
    public class VerificationDto
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 이메일 인증이 완료된 후 리디렉트할 URI
        /// </summary>
        public string RedirectUri { get; set; } = string.Empty;
    }
}
