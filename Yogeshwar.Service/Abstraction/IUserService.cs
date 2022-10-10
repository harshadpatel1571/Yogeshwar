namespace Yogeshwar.Service.Abstraction;

public interface IUserService : IDisposable
{
    Task<UserDetailDto?> GetUserByCredential(string username, string password);
}