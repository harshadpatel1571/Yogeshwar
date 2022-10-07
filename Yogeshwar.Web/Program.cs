var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

#region Built-In

services.AddControllersWithViews();
services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = new PathString("/Account/Login");
    options.LogoutPath = new PathString("/Account/Logout");
});

#endregion

#region Custom

services.AddScoped<Yogeshwar.DB.Models.YogeshwarContext>();
services.AddScoped<IUserService, UserService>()
    .AddScoped<Lazy<IUserService>>(x => new Lazy<IUserService>(() => x.GetService<IUserService>()));

#endregion

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=SignIn}/{id?}");

app.Run();