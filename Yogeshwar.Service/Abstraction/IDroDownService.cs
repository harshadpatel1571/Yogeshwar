namespace Yogeshwar.Service.Abstraction;

public interface IDropDownService : IDisposable
{
    internal Task<IList<DropDownDto<int>>> BindDropDownForAccessories();
}