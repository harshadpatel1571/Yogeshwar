namespace Yogeshwar.Helper.Extension;

internal static class FileExtension
{
    public static async Task SaveAsync(this IFormFile file, string pathWithName, CancellationToken cancellationToken = default)
    {
        var stream = new FileStream(pathWithName, FileMode.Create);
        await using var _ = stream.ConfigureAwait(false);
        await file.CopyToAsync(stream, cancellationToken).ConfigureAwait(false);
        await stream.FlushAsync(cancellationToken).ConfigureAwait(false);
    }
}