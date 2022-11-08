namespace Yogeshwar.Helper.Attribute;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class RegisterServiceAttribute : System.Attribute
{
    public RegisterServiceAttribute(ServiceLifetime serviceLifetime, Type? baseType = null,
        bool enableLazyLoading = true)
    {
        BaseType = baseType;
        ServiceLifetime = serviceLifetime;
        EnableLazyLoading = enableLazyLoading;
    }

    public Type? BaseType { get; }

    public ServiceLifetime ServiceLifetime { get; }

    public bool EnableLazyLoading { get; }
}