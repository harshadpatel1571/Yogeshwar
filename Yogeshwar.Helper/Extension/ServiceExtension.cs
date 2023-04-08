namespace Yogeshwar.Helper.Extension;

/// <summary>
/// Class ExtensionHelper.
/// </summary>
public static class ServiceExtension
{
    /// <summary>
    /// Adds the custom services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="assemblyType">Type of the assembly.</param>
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

            var genericMethod = typeof(ServiceExtension).GetMethod(nameof(EnableLazyLoading),
                BindingFlags.Static | BindingFlags.NonPublic)!;

            genericMethod = genericMethod.MakeGenericMethod(baseType ?? type.Child);

            genericMethod.CreateDelegate<Action<IServiceCollection, ServiceLifetime>>()(services, serviceLifetime);
        }
    }

    /// <summary>
    /// Enables the lazy loading.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services">The services.</param>
    /// <param name="serviceLifetime">The service lifetime.</param>
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

    /// <summary>
    /// Lazies the loader.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns>Lazy&lt;T&gt;.</returns>
    private static Lazy<T> LazyLoader<T>(IServiceProvider serviceProvider)
    {
        return new Lazy<T>(() => serviceProvider.GetService<T>()!);
    }
}