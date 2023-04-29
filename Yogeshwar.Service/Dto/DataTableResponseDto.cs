namespace Yogeshwar.Service.Dto;

internal record struct DataTableResponseDto<T>
{
    /// <summary>
    /// Gets or sets the draw.
    /// </summary>
    /// <value>The draw.</value>
    public string Draw { get; set; }

    /// <summary>
    /// Gets or sets the records filtered.
    /// </summary>
    /// <value>The records filtered.</value>
    public int RecordsFiltered { get; set; }

    /// <summary>
    /// Gets or sets the records total.
    /// </summary>
    /// <value>The records total.</value>
    public int RecordsTotal { get; set; }

    /// <summary>
    /// Gets or sets the data.
    /// </summary>
    /// <value>The data.</value>
    public IEnumerable<T> Data { get; set; }
}

/// <summary>
/// Class DataTableResponseCarrier.
/// </summary>
/// <typeparam name="T"></typeparam>
internal sealed class DataTableResponseCarrier<T>
{
    /// <summary>
    /// Gets or sets the total count.
    /// </summary>
    /// <value>The total count.</value>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the data.
    /// </summary>
    /// <value>The data.</value>
    public ICollection<T> Data { get; set; }
}
