namespace Yogeshwar.Service.Dto;

/// <summary>
/// Struct DropDownDto
/// </summary>
/// <typeparam name="T"></typeparam>
internal record struct DropDownDto<T>
{
    /// <summary>
    /// Gets or sets the key.
    /// </summary>
    /// <value>The key.</value>
    public T Key { get; set; }

    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    /// <value>The text.</value>
    public string Text { get; set; }
}