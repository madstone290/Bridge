using Bridge.Application.Common;
using Bridge.Application.Users.ReadModels;
using Bridge.Application.Users.ReadRepos;
using Bridge.Domain.Users.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Application.Users.Queries
{
    /// <summary>
    /// 아이디로 관리자를 조회한다
    /// </summary>
    public class GetAdminUserByIdQuery : IQuery<UserReadModel?>
    {
        public long Id { get; set; }
    }

    public class GetAdminUserByIdQueryHandler : QueryHandler<GetAdminUserByIdQuery, UserReadModel?>
    {
        private readonly IUserReadRepository _userRepository;

        public GetAdminUserByIdQueryHandler(IUserReadRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public override async Task<UserReadModel?> HandleQuery(GetAdminUserByIdQuery query, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAdminUserAsync(query.Id);

            return user;
        }
    }
}
