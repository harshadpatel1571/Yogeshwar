using Microsoft.EntityFrameworkCore;
using Yogeshwar.DB.Context;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

#region Built-In

services.AddControllersWithViews()
    .AddRazorRuntimeCompilation()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
    });

services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = new PathString("/Account/SignIn");
    options.LogoutPath = new PathString("/Account/SignOut");
    options.Cookie.Name = "Yogeshwar.Authentication";
});

services.AddMemoryCache();

#endregion

#region Custom

services.AddDbContextPool<YogeshwarContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
services.AddCustomServices(typeof(IUserService));
services.AddHttpContextAccessor();
services.AddHostedService<NotificationBackgroundService>();

#endregion

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseRouting();

app.UseCookiePolicy();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    // pattern: "{controller=Account}/{action=SignIn}/{id?}"
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();