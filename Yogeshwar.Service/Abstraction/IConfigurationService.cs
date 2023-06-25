namespace Yogeshwar.Service.Abstraction;

/// <summary>
/// Interface IConfigurationService
/// Extends the <see cref="System.IDisposable" />
/// </summary>
/// <seealso cref="System.IDisposable" />
public interface IConfigurationService : IDisposable
{
    /// <summary>
    /// Gets the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;ConfigurationDto&gt;&gt;.</returns>
    Task<ConfigurationDto?> GetAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Creates the or update asynchronous.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;ConfigurationDto&gt;.</returns>
    Task<ConfigurationDto> CreateOrUpdateAsync(ConfigurationDto configuration, CancellationToken cancellationToken);
}
