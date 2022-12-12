using Bridge.Shared.Constants;

namespace Bridge.Infrastructure.Identity.Services
{ 
    /// <summary>
    /// 이메일 검증기능을 제공한다.
    /// 특정 이메일 혹은 직접 검증이 힘든 이메일에 대한 검증을 수행한다.
    /// </summary>
    public interface IEmailVerificationService
    {
        /// <summary>
        /// 주어진 이메일을 검증한다.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>검증이 성공하면 true 아니면 false</returns>
        bool Verify(string email);
    }
    
    /// <summary>
    /// 기본 이메일 검증 서비스
    /// </summary>
    public class EmailVerificationService : IEmailVerificationService
    {
        public bool Verify(string email)
        {
            if (email.ToLower() == IdentityConstants.SharedAdminEmail)
                return true;

            // 모든 이메일에 대해 검증실패
            return false;
        }
    }
}
