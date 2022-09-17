using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Application.Users.ReadModels
{
    public class UserReadModel
    {
        /// <summary>
        /// 사용자 아이디
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 관리자인가?
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 사용자 이름
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// IdentityUser 아이디
        /// </summary>
        public string IdentityUserId { get; set; } = string.Empty;

    }
}
