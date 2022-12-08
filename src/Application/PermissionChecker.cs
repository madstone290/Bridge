using Bridge.Application.Common.Exceptions;
using Bridge.Application.Users;
using Bridge.Domain.Common;

namespace Bridge.Application
{
    public static class PermissionChecker
    {
        /// <summary>
        /// 관리자가 아니고 리소스 오너가 아닌 경우 권한없음 예외를 발생시킨다.
        /// </summary>
        /// <param name="aggregateRoot"></param>
        /// <param name="userId"></param>
        /// <param name="userService"></param>
        /// <exception cref="NoPermissionException"></exception>
        public static async Task ThrowIfNoPermission(IAggregateRoot aggregateRoot, string userId, IUserService userService)
        {
            if (aggregateRoot.OwnerId != userId && !(await userService.IsAdminAsync(userId)))
                throw new NoPermissionException();
        }
    }
}
