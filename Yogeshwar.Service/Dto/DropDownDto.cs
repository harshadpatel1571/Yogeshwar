namespace Yogeshwar.Service.Dto;

internal record struct DropDownDto<T>
{
    public T Key { get; set; }

    public string Text { get; set; }
}