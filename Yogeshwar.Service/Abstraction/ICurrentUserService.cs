namespace Yogeshwar.Service.Abstraction;

/// <summary>
/// Interface ICurrentUserService
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the current user identifier.
    /// </summary>
    /// <returns>System.Int32.</returns>
    int GetCurrentUserId();

    /// <summary>
    /// Gets the name of the current user.
    /// </summary>
    /// <returns>System.String.</returns>
    string GetCurrentUserName();

    /// <summary>
    /// Gets the username of the current user.
    /// </summary>
    /// <returns>System.String.</returns>
    string GetCurrentUserUserName();

    /// <summary>
    /// Gets the type of the current user.
    /// </summary>
    /// <returns>System.String.</returns>
    string GetCurrentUserType();

    /// <summary>
    /// Gets the current user email.
    /// </summary>
    /// <returns>System.String.</returns>
    string GetCurrentUserEmail();
}