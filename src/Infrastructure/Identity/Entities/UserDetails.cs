namespace Bridge.Infrastructure.Identity.Entities
{
    /// <summary>
    /// 사용자 상세정보.
    /// 사용자 계정이 아닌 기타 정보를 관리한다.
    /// </summary>
    public class UserDetails
    {
        public static UserDetails NewUserDetails(string userName, UserType userType) => new()
        {
            UserName = userName,
            UserType = userType
        };

        /// <summary>
        /// 사용자 타입
        /// </summary>
        public UserType UserType { get; private set; } = UserType.Consumer;

        /// <summary>
        /// 사용자 이름
        /// </summary>
        public string UserName { get; private set; } = string.Empty;
    }
}
