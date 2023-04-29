namespace Yogeshwar.Helper.Extension;

/// <summary>
/// Class LinqExtension.
/// </summary>
internal static class LinqExtension
{
    /// <summary>
    /// To list as an asynchronous operation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="values">The values.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
    internal static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> values, CancellationToken cancellationToken)
    {
        var list = new List<T>();

        await foreach (var item in values.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            list.Add(item);
        }

        return list;
    }

    /// <summary>
    /// Ases the list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="values">The values.</param>
    /// <returns>List&lt;T&gt;.</returns>
    internal static List<T> AsList<T>(this IEnumerable<T> values)
    {
        if (values is List<T> list)
        {
            return list;
        }

        return values.ToList();
    }

    /// <summary>
    /// Performs action for each record.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">The list.</param>
    /// <param name="action">The action.</param>
    internal static void ForEach<T>(this IList<T> list, Action<T> action)
    {
        for (var i = 0; i < list.Count; i++)
        {
            action(list[i]);
        }
    }
}
