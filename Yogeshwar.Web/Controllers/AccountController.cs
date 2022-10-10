namespace Yogeshwar.Web.Controllers;

public sealed class AccountController : Controller
{
    private readonly Lazy<IUserService> _userService;

    public AccountController(Lazy<IUserService> userService)
    {
        _userService = userService;
    }

    protected override void Dispose(bool disposing)
    {
        if (_userService.IsValueCreated)
        {
            _userService.Value.Dispose();
        }

        base.Dispose(disposing);
    }

    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(UserLoginDto userLoginDto)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("","");
            return View(userLoginDto);
        }

        var userService = _userService.Value;

        var user = await userService.GetUserByCredential(userLoginDto.UserName, userLoginDto.Password)
            .ConfigureAwait(false);

        if (user is null)
        {
            ModelState.AddModelError("", "User name or password is wrong.");
            return View(userLoginDto);
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim("Username", user.Username),
            new Claim("UserType", user.UserType.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.Now.AddMonths(1),
                    IsPersistent = userLoginDto.RememberMe
                })
            .ConfigureAwait(false);

        return RedirectToActionPermanent("Index", "Home");
    }

    [Authorize]
    public new async ValueTask<IActionResult> SignOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);

        return RedirectToActionPermanent("SignIn");
    }
}