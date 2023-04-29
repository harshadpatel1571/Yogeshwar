namespace Yogeshwar.Web.Controllers;

/// <summary>
/// Class AccountController. This class cannot be inherited.
/// Implements the <see cref="Controller" />
/// </summary>
/// <seealso cref="Controller" />
public sealed class AccountController : Controller
{
    /// <summary>
    /// The user service
    /// </summary>
    private readonly Lazy<IUserService> _userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountController" /> class.
    /// </summary>
    /// <param name="userService">The user service.</param>
    public AccountController(Lazy<IUserService> userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Releases all resources currently used by this <see cref="T:Microsoft.AspNetCore.Mvc.Controller" /> instance.
    /// </summary>
    /// <param name="disposing"><c>true</c> if this method is being invoked by the <see cref="M:Microsoft.AspNetCore.Mvc.Controller.Dispose" /> method,
    /// otherwise <c>false</c>.</param>
    protected override void Dispose(bool disposing)
    {
        if (_userService.IsValueCreated & disposing)
        {
            _userService.Value.Dispose();
        }

        base.Dispose(disposing);
    }

    /// <summary>
    /// Signs in.
    /// </summary>
    /// <returns>IActionResult.</returns>
    public IActionResult SignIn()
    {
        return View();
    }

    /// <summary>
    /// Signs in.
    /// </summary>
    /// <param name="userLoginDto">The user login dto.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>IActionResult.</returns>
    [HttpPost]
    public async Task<IActionResult> SignIn(UserLoginDto userLoginDto, [FromServices] IConfiguration configuration,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError();
            return View(userLoginDto);
        }

        var userService = _userService.Value;

        var user = await userService
            .GetUserByCredentialAsync(userLoginDto.UserName, userLoginDto.Password, cancellationToken)
            .ConfigureAwait(false);

        if (user is null)
        {
            ModelState.AddModelError("", "User name or password is wrong.");
            return View(userLoginDto);
        }

        var claims = new[]
        {
            new Claim("Id", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim("UserName", user.Username),
            new Claim("UserType", user.UserType.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.Now.AddMinutes(Convert.ToInt32(configuration["Session:TimeOut"])),
                    IsPersistent = userLoginDto.RememberMe
                })
            .ConfigureAwait(false);

        return RedirectToActionPermanent("Index", "Home");
    }

    /// <summary>
    /// Represents an event that is raised when the sign-out operation is complete.
    /// </summary>
    /// <returns>IActionResult.</returns>
    [Authorize]
    public new async ValueTask<IActionResult> SignOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme)
            .ConfigureAwait(false);

        return RedirectToActionPermanent(nameof(SignIn));
    }
}