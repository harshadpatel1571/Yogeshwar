﻿using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Yogeshwar.Helper;

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
}