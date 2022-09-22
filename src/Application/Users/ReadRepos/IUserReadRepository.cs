using Bridge.Application.Common;
using Bridge.Application.Users.ReadModels;
using Bridge.Domain.Users.Entities;

namespace Bridge.Application.Users.ReadRepos
{
    public interface IUserReadRepository : IReadRepository<User, UserReadModel>
    {
        /// <summary>
        /// 아이디로 사용자 조회
        /// </summary>
        /// <param name="id">사용자 아이디</param>
        /// <returns></returns>
        Task<UserReadModel?> GetUserAsync(long id);

        /// <summary>
        /// 아이디로 관리자 조회
        /// </summary>
        /// <param name="id">관리자 아이디</param>
        /// <returns></returns>
        Task<UserReadModel?> GetAdminUserAsync(long id);
    }
}
