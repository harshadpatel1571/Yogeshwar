namespace Yogeshwar.Service.Abstraction;

/// <summary>
/// Interface ICustomerService
/// Extends the <see cref="IDisposable" />
/// </summary>
/// <seealso cref="IDisposable" />
public interface ICustomerService : IDisposable
{
    /// <summary>
    /// Gets by filter asynchronous.
    /// </summary>
    /// <param name="filterDto">The filter dto.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;DataTableResponseCarrier&lt;CustomerDto&gt;&gt;.</returns>
    internal Task<DataTableResponseCarrier<CustomerDto>> GetByFilterAsync(DataTableFilterDto filterDto, CancellationToken cancellationToken);

    /// <summary>
    /// Creates the or update asynchronous.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;System.Int32&gt;.</returns>
    Task<int> CreateOrUpdateAsync(CustomerDto customer, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;System.Nullable&lt;CustomerDto&gt;&gt;.</returns>
    Task<CustomerDto?> GetSingleAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;System.Int32&gt;.</returns>
    Task<int> DeleteAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the image asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>ValueTask&lt;System.Boolean&gt;.</returns>
    ValueTask<bool> DeleteImageAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Actives and in active record asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;OneOf&lt;System.Boolean, NotFound&gt;&gt;.</returns>
    Task<OneOf<bool, NotFound>> ActiveInActiveRecordAsync(int id, CancellationToken cancellationToken);
}
