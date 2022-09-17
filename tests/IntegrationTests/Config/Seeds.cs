using Bridge.Application.Users.ReadModels;
using Bridge.Domain.Users.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.IntegrationTests.Config
{
    public static class Seeds
    {
        static Seeds()
        {
            RootUser = User.Create("root", "root");
            typeof(User).GetProperty(nameof(User.IsAdmin))?.SetValue(RootUser, true);
        }

        public static User RootUser { get; }
    }
}
