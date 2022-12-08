namespace Bridge.Application.Users
{
    public interface IUserService
    {
        Task<bool> IsAdminAsync(string userId);
    }
}
