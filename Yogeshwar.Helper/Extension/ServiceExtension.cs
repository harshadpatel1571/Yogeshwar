namespace Yogeshwar.Helper.Extension;

public static class ExtensionHelper
{
    public static void AddCustomServices(this IServiceCollection services, Type assemblyType)
    {
        var types = assemblyType.Assembly.GetTypes()
            .Where(x => x.GetCustomAttribute<RegisterServiceAttribute>() is not null)
            .Select(x => new { Child = x, Attribute = x.GetCustomAttribute<RegisterServiceAttribute>() })
            .ToArray();

        foreach (var type in types)
        {
            var baseType = type.Attribute!.ParentType;
            var serviceLifetime = type.Attribute.ServiceLifetime;

            if (baseType is not null)
            {
                _ = serviceLifetime switch
                {
                    ServiceLifetime.Scoped => services.AddScoped(baseType, type.Child),
                    ServiceLifetime.Singleton => services.AddSingleton(baseType, type.Child),
                    ServiceLifetime.Transient => services.AddTransient(baseType, type.Child),
                    _ => throw new InvalidEnumArgumentException(nameof(serviceLifetime))
                };
            }
            else
            {
                _ = serviceLifetime switch
                {
                    ServiceLifetime.Scoped => services.AddScoped(type.Child),
                    ServiceLifetime.Singleton => services.AddSingleton(type.Child),
                    ServiceLifetime.Transient => services.AddTransient(type.Child),
                    _ => throw new InvalidEnumArgumentException(nameof(serviceLifetime))
                };
            }

            if (!type.Attribute.EnableLazyLoading)
            {
                continue;
            }

            var genericMethod = typeof(ExtensionHelper).GetMethod(nameof(EnableLazyLoading),
                BindingFlags.Static | BindingFlags.NonPublic)!;

            genericMethod = genericMethod.MakeGenericMethod(baseType ?? type.Child);

            genericMethod.CreateDelegate<Action<IServiceCollection, ServiceLifetime>>()(services, serviceLifetime);
        }
    }

    private static void EnableLazyLoading<T>(IServiceCollection services, ServiceLifetime serviceLifetime)
    {
        _ = serviceLifetime switch
        {
            ServiceLifetime.Scoped => services.AddScoped(LazyLoader<T>),
            ServiceLifetime.Singleton => services.AddSingleton(LazyLoader<T>),
            ServiceLifetime.Transient => services.AddTransient(LazyLoader<T>),
            _ => throw new InvalidEnumArgumentException(nameof(serviceLifetime))
        };
    }

    private static Lazy<T> LazyLoader<T>(IServiceProvider serviceProvider)
    {
        return new Lazy<T>(() => serviceProvider.GetService<T>()!);
    }
}