namespace Yogeshwar.Service.Abstraction;

public interface IDropDownService : IDisposable
{
    /// <summary>
    /// Binds the drop down for accessories asynchronous.
    /// </summary>
    /// <returns></returns>
    internal Task<IList<DropDownDto<int>>> BindDropDownForAccessoriesAsync();

    /// <summary>
    /// Binds the drop down for categories asynchronous.
    /// </summary>
    /// <returns></returns>
    internal Task<IList<DropDownDto<int>>> BindDropDownForCategoriesAsync();

    /// <summary>
    /// Binds the drop down for orders asynchronous.
    /// </summary>
    /// <returns></returns>
    internal Task<IList<DropDownDto<int>>> BindDropDownForOrdersAsync();

    /// <summary>
    /// Binds the drop down for customers asynchronous.
    /// </summary>
    /// <returns></returns>
    internal Task<IList<DropDownDto<int>>> BindDropDownForCustomersAsync();

    /// <summary>
    /// Binds the drop down for products asynchronous.
    /// </summary>
    /// <returns></returns>
    internal Task<IList<DropDownDto<int>>> BindDropDownForProductsAsync();

    /// <summary>
    /// Binds the drop down for status.
    /// </summary>
    /// <returns></returns>
    internal IList<DropDownDto<byte>> BindDropDownForOrderStatus();

    /// <summary>
    /// Binds the drop down for order status.
    /// </summary>
    /// <returns></returns>
    internal IList<DropDownDto<byte>> BindDropDownForOrderDetailStatus();

    /// <summary>
    /// Binds the drop down for service.
    /// </summary>
    /// <returns></returns>
    internal IList<DropDownDto<byte>> BindDropDownForService();
}