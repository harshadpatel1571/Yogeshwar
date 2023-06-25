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
    Task<DataTableResponseCarrier<CustomerDto>> GetByFilterAsync(DataTableFilterDto filterDto, CancellationToken cancellationToken);

    /// <summary>
    /// Creates the or update asynchronous.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;CustomerDto&gt;&gt;.</returns>
    Task<CustomerDto?> CreateOrUpdateAsync(CustomerDto customer, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the by identifier asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;CustomerDto&gt;&gt;.</returns>
    Task<CustomerDto?> GetByIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;CustomerDto&gt;&gt;.</returns>
    Task<CustomerDto?> DeleteAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the image asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;CustomerDto&gt;&gt;.</returns>
    Task<CustomerDto?> DeleteImageAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Actives the in active record asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;CustomerDto&gt;&gt;.</returns>
    Task<CustomerDto?> ActiveInActiveRecordAsync(int id, CancellationToken cancellationToken);
}