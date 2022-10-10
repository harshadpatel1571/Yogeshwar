namespace Yogeshwar.Helper.Extension;

internal static class DataExtractor
{
    public static DataTableFilterDto Extract(HttpRequest request)
    {
        var sortColumn = request.Form["columns[" + request.Form["order[0][column]"][0] + "][name]"][0];

        return new DataTableFilterDto
        {
            Draw = request.Form["draw"][0],
            Skip = Convert.ToInt32(request.Form["start"][0]),
            Take = Convert.ToInt32(request.Form["length"][0]),
            SortColumn = sortColumn.Contains(' ')
                ? string.Join(null, sortColumn.Split(' '))
                : sortColumn,
            SortOrder = request.Form["order[0][dir]"][0],
            SearchValue = request.Form["search[value]"][0]
        };
    }
}

internal class DataTableFilterDto
{
    public string Draw { get; set; }

    public int Skip { get; set; }

    public int Take { get; set; }

    public string SortColumn { get; set; }

    public string SortOrder { get; set; }

    public string? SearchValue { get; set; }
}