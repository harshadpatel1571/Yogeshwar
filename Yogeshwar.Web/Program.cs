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
services.AddScoped<IDropDownService, DropDownService>();
services.AddScoped<IUserService, UserService>()
    .AddScoped(x => new Lazy<IUserService>(() => x.GetService<IUserService>()!));
services.AddScoped<ICustomerService, CustomerService>()
    .AddScoped(x => new Lazy<ICustomerService>(() => x.GetService<ICustomerService>()!));
services.AddScoped<IAccessoriesService, AccessoriesService>()
    .AddScoped(x => new Lazy<IAccessoriesService>(() => x.GetService<IAccessoriesService>()!));
services.AddScoped<IProductService, ProductService>()
    .AddScoped(x => new Lazy<IProductService>(() => x.GetService<IProductService>()!));
services.AddScoped<IOrderService, OrderService>()
    .AddScoped(x => new Lazy<IOrderService>(() => x.GetService<IOrderService>()!));

#endregion

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseRouting();

app.UseCookiePolicy();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    // pattern: "{controller=Account}/{action=SignIn}/{id?}"
    pattern: "{controller=Order}/{action=Index}/{id?}"
);

app.Run();