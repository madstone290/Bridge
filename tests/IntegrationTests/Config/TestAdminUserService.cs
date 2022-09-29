using Bridge.Infrastructure.Identity.Services;

namespace Bridge.IntegrationTests.Config
{
    public class TestAdminUserService : IAdminUserService
    {
        public bool VerifyAdmin(string email)
        {
            return email == TestUser.Admin.Email;
        }
    }
}
