namespace Yogeshwar.Helper.Attribute;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class RegisterServiceAttribute : System.Attribute
{
    public RegisterServiceAttribute(ServiceLifetime serviceLifetime, Type? parentType = null,
        bool enableLazyLoading = true)
    {
        ParentType = parentType;
        ServiceLifetime = serviceLifetime;
        EnableLazyLoading = enableLazyLoading;
    }

    public Type? ParentType { get; }

    public ServiceLifetime ServiceLifetime { get; }

    public bool EnableLazyLoading { get; }
}