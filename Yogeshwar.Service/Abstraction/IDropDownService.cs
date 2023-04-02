namespace Yogeshwar.Service.Abstraction;

/// <summary>
/// Interface IDropDownService
/// Extends the <see cref="IDisposable" />
/// </summary>
/// <seealso cref="IDisposable" />
public interface IDropDownService : IDisposable
{
    /// <summary>
    /// Binds the drop down for accessories asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IList&lt;DropDownDto&lt;System.Int32&gt;&gt;&gt;.</returns>
    internal Task<IList<DropDownDto<int>>> BindDropDownForAccessoriesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Binds the drop down for categories asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;IList&lt;DropDownDto&lt;System.Int32&gt;&gt;&gt;.</returns>
    internal Task<IList<DropDownDto<int>>> BindDropDownForCategoriesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Binds the drop down for orders asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;IList&lt;DropDownDto&lt;System.Int32&gt;&gt;&gt;.</returns>
    internal Task<IList<DropDownDto<int>>> BindDropDownForOrdersAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Binds the drop down for customers asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="ids">The ids.</param>
    /// <returns>Task&lt;IList&lt;DropDownDto&lt;System.Int32&gt;&gt;&gt;.</returns>
    internal Task<IList<DropDownDto<int>>> BindDropDownForCustomersAsync(CancellationToken cancellationToken, params int[] ids);

    /// <summary>
    /// Binds the drop down for products asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task&lt;IList&lt;DropDownDto&lt;System.Int32&gt;&gt;&gt;.</returns>
    internal Task<IList<DropDownDto<int>>> BindDropDownForProductsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Binds the drop down for status.
    /// </summary>
    /// <returns>IList&lt;DropDownDto&lt;System.Byte&gt;&gt;.</returns>
    internal IList<DropDownDto<byte>> BindDropDownForOrderStatus();

    /// <summary>
    /// Binds the drop down for order status.
    /// </summary>
    /// <returns>IList&lt;DropDownDto&lt;System.Byte&gt;&gt;.</returns>
    internal IList<DropDownDto<byte>> BindDropDownForOrderDetailStatus();

    /// <summary>
    /// Binds the drop down for service.
    /// </summary>
    /// <returns>IList&lt;DropDownDto&lt;System.Byte&gt;&gt;.</returns>
    internal IList<DropDownDto<byte>> BindDropDownForService();
}