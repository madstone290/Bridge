using Bridge.Domain.Users.Entities;

namespace Bridge.UnitTests.DomainTests.Builders
{
    public class UserBuilder
    {
        public User BuildNormalUser(string? name = null)
        {
            var user = User.Create(string.Empty, name ?? Guid.NewGuid().ToString());
            return user;
        }

        public User BuildAdminUser(string? name = null)
        {
            var superUser = BuildSuperUser();
            var admin = User.CreateAdmin(superUser, string.Empty, name ?? Guid.NewGuid().ToString());
            return admin;
        }



        static User BuildSuperUser(string? name = null)
        {
            var user = User.Create(string.Empty, name ?? Guid.NewGuid().ToString());
            typeof(User).GetProperty(nameof(User.IsAdmin))?.SetValue(user, true);

            return user;
        }
    }
}
