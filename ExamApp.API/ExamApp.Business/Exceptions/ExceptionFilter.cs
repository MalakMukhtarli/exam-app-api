using ExamApp.Data.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Storage;

namespace ExamApp.Business.Exceptions;

public class ExceptionFilter : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    private readonly AppDbContext _dbContext;

    private IDbContextTransaction? Transaction => _dbContext.Database.CurrentTransaction;

    public ExceptionFilter(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        // Register known exception types and handlers.
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            {typeof(ValidationException), HandleValidationException},
            {typeof(NotFoundException), HandleNotFoundException},
            {typeof(BadHttpRequestException), HandleBadHttpRequestException},
            {typeof(DuplicateConflictException), HandleDuplicateConflictException},
        };
    }

    public override async Task OnExceptionAsync(ExceptionContext context)
    {
        HandleException(context);

        if (Transaction is not null)
            await _dbContext.Database.RollbackTransactionAsync();

        await base.OnExceptionAsync(context);
    }

    private void HandleException(ExceptionContext context)
    {
        while (context.Exception.InnerException is not null)
        {
            context.Exception = context.Exception.InnerException;
        }

        var type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException(context);
            return;
        }

        HandleUnknownException(context);
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = context.Exception as ValidationException;

        var details = new ValidationProblemDetails(exception?.Errors);
        context.ExceptionHandled = true;
        context.Result = new UnprocessableEntityObjectResult(details);
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        var details = new ProblemDetails()
        {
            Status = StatusCodes.Status404NotFound,
            Title = context.Exception.Message
        };

        context.ExceptionHandled = true;
        context.Result = new NotFoundObjectResult(details);
    }

    private void HandleDuplicateConflictException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = context.Exception.Message
        };

        context.ExceptionHandled = true;
        context.Result = new ObjectResult(details);
    }

    private void HandleInvalidModelStateException(ExceptionContext context)
    {
        var details = new ValidationProblemDetails(context.ModelState);

        context.ExceptionHandled = true;
        context.Result = new BadRequestObjectResult(details);
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Əməliyyat zamanı xəta baş verdi."
        };

        context.ExceptionHandled = true;
        context.Result = new ObjectResult(details);
    }

    private void HandleBadHttpRequestException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = context.Exception.Message
        };

        context.ExceptionHandled = true;
        context.Result = new ObjectResult(details);
    }
}