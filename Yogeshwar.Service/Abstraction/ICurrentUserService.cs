namespace Yogeshwar.Service.Abstraction;

public interface ICurrentUserService
{
    /// <summary>
    /// Gets the current user identifier.
    /// </summary>
    /// <returns></returns>
    int GetCurrentUserId();

    /// <summary>
    /// Gets the name of the current user.
    /// </summary>
    /// <returns></returns>
    string GetCurrentUserName();

    /// <summary>
    /// Gets the username of the current user.
    /// </summary>
    /// <returns></returns>
    string GetCurrentUserUserName();

    /// <summary>
    /// Gets the type of the current user.
    /// </summary>
    /// <returns></returns>
    string GetCurrentUserType();

    /// <summary>
    /// Gets the current user email.
    /// </summary>
    /// <returns></returns>
    string GetCurrentUserEmail();
}