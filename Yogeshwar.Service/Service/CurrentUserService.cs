namespace Yogeshwar.Service.Service;

/// <summary>
/// Class CurrentUserService.
/// Implements the <see cref="ICurrentUserService" />
/// </summary>
/// <seealso cref="ICurrentUserService" />
[RegisterService(ServiceLifetime.Scoped, typeof(ICurrentUserService))]
internal sealed class CurrentUserService : ICurrentUserService
{
    /// <summary>
    /// The claims principal
    /// </summary>
    private readonly ClaimsPrincipal _claimsPrincipal;

    /// <summary>
    /// Initializes a new instance of the <see cref="CurrentUserService" /> class.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    public CurrentUserService(IHttpContextAccessor httpContext)
    {
        if (httpContext?.HttpContext?.User == null)
            throw new ArgumentNullException(nameof(httpContext));
        
        _claimsPrincipal = httpContext.HttpContext!.User;
    }

    /// <summary>
    /// Gets the current user identifier.
    /// </summary>
    /// <returns>System.Int32.</returns>
    public int GetCurrentUserId() => int.Parse(_claimsPrincipal.FindFirst("Id")!.Value);

    /// <summary>
    /// Gets the name of the current user.
    /// </summary>
    /// <returns>System.String.</returns>
    public string GetCurrentUserName() => _claimsPrincipal.FindFirst(ClaimTypes.Name)!.Value;

    /// <summary>
    /// Gets the user name of the current user.
    /// </summary>
    /// <returns>System.String.</returns>
    public string GetCurrentUserUserName() => _claimsPrincipal.FindFirst("UserName")!.Value;

    /// <summary>
    /// Gets the type of the current user.
    /// </summary>
    /// <returns>System.String.</returns>
    public string GetCurrentUserType() => _claimsPrincipal.FindFirst("UserType")!.Value;

    /// <summary>
    /// Gets the current user email.
    /// </summary>
    /// <returns>System.String.</returns>
    public string GetCurrentUserEmail() => _claimsPrincipal.FindFirst(ClaimTypes.Email)!.Value;
}