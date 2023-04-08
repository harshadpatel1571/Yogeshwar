namespace Yogeshwar.Helper.Extension;

/// <summary>
/// Class FileExtension.
/// </summary>
internal static class FileExtension
{
    /// <summary>
    /// Save as an asynchronous operation.
    /// </summary>
    /// <param name="file">The file.</param>
    /// <param name="pathWithName">Name of the path with.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public static async Task SaveAsync(this IFormFile file, string pathWithName,
        CancellationToken cancellationToken)
    {
        var stream = new FileStream(pathWithName, FileMode.Create);
        await using var _ = stream.ConfigureAwait(false);
        await file.CopyToAsync(stream, cancellationToken).ConfigureAwait(false);
        await stream.FlushAsync(cancellationToken).ConfigureAwait(false);
    }
}