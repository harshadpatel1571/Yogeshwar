﻿namespace Yogeshwar.Service.Abstraction;

public interface IDropDownService : IDisposable
{
    internal Task<IList<DropDownDto<int>>> BindDropDownForAccessoriesAsync();

    internal Task<IList<DropDownDto<int>>> BindDropDownForCustomersAsync();
    
    internal Task<IList<DropDownDto<int>>> BindDropDownForProductsAsync();

    internal IList<DropDownDto<string>> BindDropDownForStatus();
}