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
    internal Task<DataTableResponseCarrier<AccessoriesDto>> GetByFilterAsync(DataTableFilterDto filterDto,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets the by ids asynchronous.
    /// </summary>
    /// <param name="ids">The ids.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IList&lt;AccessoriesDto&gt;&gt;.</returns>
    Task<IList<AccessoriesDto>> GetByIdsAsync(IList<int> ids, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the single asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;System.Nullable&lt;AccessoriesDto&gt;&gt;.</returns>
    Task<AccessoriesDto?> GetSingleAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Creates or update asynchronous.
    /// </summary>
    /// <param name="customer">The customer.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;System.Int32&gt;.</returns>
    Task<int> CreateOrUpdateAsync(AccessoriesDto customer, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the image asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>ValueTask&lt;System.Boolean&gt;.</returns>
    ValueTask<bool> DeleteImageAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;System.Int32&gt;.</returns>
    internal Task<int> DeleteAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Actives and in active record asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;OneOf&lt;System.Boolean, NotFound&gt;&gt;.</returns>
    Task<OneOf<bool, NotFound>> ActiveInActiveRecordAsync(int id, CancellationToken cancellationToken);
}