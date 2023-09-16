using ExamApp.Business.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExamApp.API.Attributes;

public class ValidationAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        //before controller
        if (!context.ModelState.IsValid)
        {
            var errorsInModelState = context.ModelState.Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage)).ToArray();

            var exception = new ValidationException();

            foreach (var error in errorsInModelState)
            {
                exception.Errors.Add(error.Key, error.Value.ToArray());
            }

            throw exception;
        }

        await next();
    }
}