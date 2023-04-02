namespace Yogeshwar.Helper.Attribute;

/// <summary>
/// Class RegisterServiceAttribute. This class cannot be inherited.
/// Implements the <see cref="System.Attribute" />
/// </summary>
/// <seealso cref="System.Attribute" />
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class RegisterServiceAttribute : System.Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterServiceAttribute"/> class.
    /// </summary>
    /// <param name="serviceLifetime">The service lifetime.</param>
    /// <param name="parentType">Type of the parent.</param>
    /// <param name="enableLazyLoading">if set to <c>true</c> [enable lazy loading].</param>
    public RegisterServiceAttribute(ServiceLifetime serviceLifetime, Type? parentType = null,
        bool enableLazyLoading = true)
    {
        ParentType = parentType;
        ServiceLifetime = serviceLifetime;
        EnableLazyLoading = enableLazyLoading;
    }

    /// <summary>
    /// Gets the type of the parent.
    /// </summary>
    /// <value>The type of the parent.</value>
    public Type? ParentType { get; }

    /// <summary>
    /// Gets the service lifetime.
    /// </summary>
    /// <value>The service lifetime.</value>
    public ServiceLifetime ServiceLifetime { get; }

    /// <summary>
    /// Gets a value indicating whether [enable lazy loading].
    /// </summary>
    /// <value><c>true</c> if [enable lazy loading]; otherwise, <c>false</c>.</value>
    public bool EnableLazyLoading { get; }
}