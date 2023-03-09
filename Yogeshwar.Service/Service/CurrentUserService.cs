using System.Security.Claims;

namespace Yogeshwar.Service.Service;

[RegisterService(ServiceLifetime.Scoped, typeof(ICurrentUserService))]
public class CurrentUserService : ICurrentUserService
{
    private readonly ClaimsPrincipal _claimsPrincipal;

    /// <summary>
    /// Initializes a new instance of the <see cref="CurrentUserService"/> class.
    /// </summary>
    /// <param name="claimsPrincipal">The claims principal.</param>
    public CurrentUserService(ClaimsPrincipal claimsPrincipal)
    {
        _claimsPrincipal = claimsPrincipal;
    }

    /// <summary>
    /// Gets the current user identifier.
    /// </summary>
    /// <returns></returns>
    public int GetCurrentUserId() => int.Parse(_claimsPrincipal.FindFirst("Id")!.Value);

    /// <summary>
    /// Gets the name of the current user.
    /// </summary>
    /// <returns></returns>
    public string GetCurrentUserName() => _claimsPrincipal.FindFirst(ClaimTypes.Name)!.Value;

    /// <summary>
    /// Gets the username of the current user.
    /// </summary>
    /// <returns></returns>
    public string GetCurrentUserUserName() => _claimsPrincipal.FindFirst("UserName")!.Value;

    /// <summary>
    /// Gets the type of the current user.
    /// </summary>
    /// <returns></returns>
    public string GetCurrentUserType() => _claimsPrincipal.FindFirst("UserType")!.Value;

    /// <summary>
    /// Gets the current user email.
    /// </summary>
    /// <returns></returns>
    public string GetCurrentUserEmail() => _claimsPrincipal.FindFirst(ClaimTypes.Email)!.Value;
}