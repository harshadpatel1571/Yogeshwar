namespace Yogeshwar.Service.Abstraction;

/// <summary>
/// Interface IUserService
/// Extends the <see cref="IDisposable" />
/// </summary>
/// <seealso cref="IDisposable" />
public interface IUserService : IDisposable
{
    /// <summary>
    /// Gets the user by credential.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="password">The password.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;System.Nullable&lt;UserDetailDto&gt;&gt;.</returns>
    Task<UserDetailDto?> GetUserByCredentialAsync(string username, string password, CancellationToken cancellationToken);
}