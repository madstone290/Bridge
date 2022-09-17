using Bridge.Domain.Common;
using Bridge.Domain.Common.Exceptions;
using Bridge.Domain.Users.Exceptions;

namespace Bridge.Domain.Users.Entities
{
    public class User : AggregateRoot
    {
        private User() { }
        private User(string identityUserId, string name, bool isAdmin)
        {
            IdentityUserId = identityUserId;
            SetName(name);
            IsAdmin = isAdmin;
        }


        public static User Create(string identityUserId, string name)
        {
            return new User(identityUserId, name, false);
        }

        public static User CreateAdmin(User creator, string identityUserId, string name)
        {
            if(!creator.IsAdmin)
                throw new NoPermissionException();

            return new User(identityUserId, name, true);
        }

        /// <summary>
        /// 관리자인가?
        /// </summary>
        public bool IsAdmin { get; private set; }

        /// <summary>
        /// 사용자 이름
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// IdentityUser 아이디
        /// </summary>
        public string IdentityUserId { get; private set; } = string.Empty;

        /// <summary>
        /// 사용자 이름을 변경한다.
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="InvalidUserNameException"></exception>
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidUserNameException();

            if (Name == name)
                return;

            Name = name;
        }
     
    }
}
