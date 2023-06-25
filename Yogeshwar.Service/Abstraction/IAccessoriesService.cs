namespace Yogeshwar.Service.Abstraction;

/// <summary>
/// Interface IAccessoriesService
/// Extends the <see cref="IDisposable" />
/// </summary>
/// <seealso cref="IDisposable" />
public interface IAccessoriesService : IDisposable
{
    /// <summary>
    /// Gets by filter asynchronous.
    /// </summary>
    /// <param name="filterDto">The filter dto.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;DataTableResponseCarrier&lt;AccessoriesDto&gt;&gt;.</returns>
    Task<DataTableResponseCarrier<AccessoriesDto>> GetByFilterAsync(DataTableFilterDto filterDto,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets the by ids asynchronous.
    /// </summary>
    /// <param name="ids">The ids.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IList&lt;AccessoriesDto&gt;&gt;.</returns>
    Task<IList<AccessoriesDto>> GetByIdsAsync(IList<int> ids, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the by identifier asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;AccessoriesDto&gt;&gt;.</returns>
    Task<AccessoriesDto?> GetByIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Creates or update asynchronous.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;System.Int32&gt;.</returns>
    Task<AccessoriesDto?> CreateOrUpdateAsync(AccessoriesDto customer, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the image asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;AccessoriesDto&gt;&gt;.</returns>
    Task<AccessoriesDto?> DeleteImageAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;AccessoriesDto&gt;&gt;.</returns>
    Task<AccessoriesDto?> DeleteAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Actives the in active record asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;AccessoriesDto&gt;&gt;.</returns>
    Task<AccessoriesDto?> ActiveInActiveRecordAsync(int id, CancellationToken cancellationToken);
}