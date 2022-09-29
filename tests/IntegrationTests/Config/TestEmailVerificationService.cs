using Bridge.Infrastructure.Identity.Services;

namespace Bridge.IntegrationTests.Config
{
    public class TestEmailVerificationService : IEmailVerificationService
    {
        public bool Verify(string email)
        {
            // 모든 이메일에 대해 인증완료
            return true;
        }
    }
}
