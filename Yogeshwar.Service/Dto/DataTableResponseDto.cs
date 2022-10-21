namespace Yogeshwar.Service.Dto;

internal record struct DataTableResponseDto<T>
{
    public string Draw { get; set; }

    public int RecordsFiltered { get; set; }

    public int RecordsTotal { get; set; }

    public IEnumerable<T> Data { get; set; }
}

internal class DataTableResponseCarrier<T>
{
    public int TotalCount { get; set; }

    public ICollection<T> Data { get; set; }
}
