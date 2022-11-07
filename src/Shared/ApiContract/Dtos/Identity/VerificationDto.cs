using System.ComponentModel.DataAnnotations;

namespace Bridge.Shared.ApiContract.Dtos.Identity
{
    public class VerificationDto
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 이메일 인증이 완료된 후 리디렉트할 URI. 공백인 경우 이메일 검증 후 리디렉트 대신 환영html을 출력한다.
        /// </summary>
        public string? RedirectUri { get; set; }
    }
}
