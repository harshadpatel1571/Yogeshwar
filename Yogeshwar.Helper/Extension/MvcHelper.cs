namespace Yogeshwar.Helper.Extension;

internal static class MvcHelper
{
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