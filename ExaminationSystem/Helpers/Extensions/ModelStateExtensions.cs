using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExaminationSystem.Helpers.Extensions;

public static class ModelStateExtensions
{
    public static string GetErrorMessages(this ModelStateDictionary modelState) 
    {
        return string.Join(" | ",
            modelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
        );
    }
}
