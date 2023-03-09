namespace Yogeshwar.Service.Abstraction;

public interface IUserService : IDisposable
{
    /// <summary>
    /// Gets the user by credential.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="password">The password.</param>
    /// <returns></returns>
    Task<UserDetailDto?> GetUserByCredential(string username, string password);
}