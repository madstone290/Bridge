using Bridge.Shared.Constants;

namespace Bridge.Infrastructure.Identity.Services
{
    /// <summary>
    /// 관리자 등록에 필요한 기능을 제공한다.
    /// </summary>
    public interface IAdminUserService
    {
        /// <summary>
        /// 관리자인지 검증한다.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        bool VerifyAdmin(string email);
    }

    public class AdminUserService : IAdminUserService
    {
        public bool VerifyAdmin(string email)
        {
            if(email.ToLower() == IdentityConstants.MasterEmail)
                return true;
            if (email.ToLower() == IdentityConstants.SharedAdminEmail)
                return true;

            return false;
        }
    }
}