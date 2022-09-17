using Bridge.Application.Common;
using Bridge.Application.Common.Exceptions.EntityNotFoundExceptions;
using Bridge.Domain.Users.Entities;
using Bridge.Domain.Users.Repos;

namespace Bridge.Application.Users.Commands
{
    /// <summary>
    /// 신규 관리자를 생성한다
    /// </summary>
    public class CreateAdminUserCommand : ICommand<long>
    {
        /// <summary>
        /// 신규 관리자를 생성하는 기존 관리자의 IdentityUser아이디
        /// </summary>
        public string CreatorIdentityUserId { get; set; } = string.Empty;

        /// <summary>
        /// 신규 관리자 이름
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }

    public class CreateAdminUserCommandHandler : CommandHandler<CreateAdminUserCommand, long>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateAdminUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task<long> HandleCommand(CreateAdminUserCommand command, CancellationToken cancellationToken)
        {
            var superUser = await _userRepository.FindByIdentityUserId(command.CreatorIdentityUserId)
                ?? throw new UserNotFoundException(new { command.CreatorIdentityUserId });

            // TODO 실제 IdentityUser 적용할 것
            var identityUserId = "";
            var admin = User.CreateAdmin(superUser, identityUserId, command.Name);
            await _userRepository.AddAsync(admin);

            await _unitOfWork.CommitAsync();
            return admin.Id;
        }
    }
}
