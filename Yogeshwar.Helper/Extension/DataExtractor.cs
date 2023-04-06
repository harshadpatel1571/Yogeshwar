namespace Yogeshwar.Helper.Extension;

/// <summary>
/// Class DataExtractor.
/// </summary>
internal static class DataExtractor
{
    /// <summary>
    /// Extracts the specified request.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns>DataTableFilterDto.</returns>
    public static DataTableFilterDto Extract(HttpRequest request)
    {
        try
        {
            var sortColumn = request.Form["columns[" + request.Form["order[0][column]"][0] + "][name]"][0]!;

            string sortColumnActual;

            var sortOrder = request.Form["order[0][dir]"][0]!;

            if (sortColumn.Equals("id", StringComparison.InvariantCultureIgnoreCase))
            {
                sortColumnActual = "CreatedDate";
                sortOrder = "desc";
            }
            else
            {
                sortColumnActual = sortColumn!.Contains(' ')
                    ? string.Join(null, sortColumn.Split(' '))
                    : sortColumn;
            }

            return new DataTableFilterDto
            {
                Draw = request.Form["draw"][0]!,
                Skip = Convert.ToInt32(request.Form["start"][0]),
                Take = Convert.ToInt32(request.Form["length"][0]),
                SortColumn = sortColumnActual,
                SortOrder = sortOrder,
                SearchValue = request.Form["search[value]"][0]
            };
        }
        catch
        {
            return new DataTableFilterDto
            {
                Draw = "1",
                Skip = 0,
                Take = 10,
                SortColumn = "CreatedDate",
                SortOrder = "desc",
                SearchValue = null
            };
        }
    }
}

/// <summary>
/// Class DataTableFilterDto.
/// </summary>
internal class DataTableFilterDto
{
    /// <summary>
    /// Gets or sets the draw.
    /// </summary>
    /// <value>The draw.</value>
    public string Draw { get; set; }

    /// <summary>
    /// Gets or sets the skip.
    /// </summary>
    /// <value>The skip.</value>
    public int Skip { get; set; }

    /// <summary>
    /// Gets or sets the take.
    /// </summary>
    /// <value>The take.</value>
    public int Take { get; set; }

    /// <summary>
    /// Gets or sets the sort column.
    /// </summary>
    /// <value>The sort column.</value>
    public string SortColumn { get; set; }

    /// <summary>
    /// Gets or sets the sort order.
    /// </summary>
    /// <value>The sort order.</value>
    public string SortOrder { get; set; }

    /// <summary>
    /// Gets or sets the search value.
    /// </summary>
    /// <value>The search value.</value>
    public string? SearchValue { get; set; }
}