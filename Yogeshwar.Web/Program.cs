var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

#region Built-In

services.AddControllersWithViews();
services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = new PathString("/Account/SignIn");
    options.LogoutPath = new PathString("/Account/SignOut");
    options.Cookie.Name = "Yogeshwar.Authentication";
});

#endregion

#region Custom

services.AddScoped<Yogeshwar.DB.Models.YogeshwarContext>();
services.AddScoped<IUserService, UserService>()
    .AddScoped(x => new Lazy<IUserService>(() => x.GetService<IUserService>()!));
services.AddScoped<ICustomerService, CustomerService>()
    .AddScoped(x => new Lazy<ICustomerService>(() => x.GetService<ICustomerService>()!));
services.AddScoped<IAccessoriesService, AccessoriesService>()
    .AddScoped(x => new Lazy<IAccessoriesService>(() => x.GetService<IAccessoriesService>()!));

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
    // pattern: "{controller=Account}/{action=SignIn}/{id?}"
    pattern: "{controller=Accessories}/{action=Index}/{id?}"
);

app.Run();