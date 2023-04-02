namespace Yogeshwar.Helper.Extension;

/// <summary>
/// Class MvcHelper.
/// </summary>
internal static class MvcHelper
{
    /// <summary>
    /// Adds the model error.
    /// </summary>
    /// <param name="modelState">State of the model.</param>
    public static void AddModelError(this ModelStateDictionary modelState)
    {
        var keyError = modelState
            .Where(x => x.Value?.ValidationState == ModelValidationState.Invalid)
            .Select(x => new
            {
                x.Key,
                Message = x.Value.Errors
                    .Select(c => c.ErrorMessage).FirstOrDefault()
            });

        foreach (var item in keyError)
        {
            modelState.AddModelError(item.Key, item.Message!);
        }
    }

    /// <summary>
    /// Gets the key errors.
    /// </summary>
    /// <param name="modelState">State of the model.</param>
    /// <returns>System.Object.</returns>
    public static object GetKeyErrors(this ModelStateDictionary modelState)
    {
        var keyError = modelState
            .Where(x => x.Value?.ValidationState == ModelValidationState.Invalid)
            .Select(x => new
            {
                x.Key,
                Message = x.Value.Errors
                    .Select(c => c.ErrorMessage).FirstOrDefault()
            });

        return keyError;
    }
}