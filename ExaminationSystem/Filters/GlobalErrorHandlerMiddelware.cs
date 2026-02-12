
namespace ExaminationSystem.Filters;

public class GlobalErrorHandlerMiddelware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex) 
        {
            //TODO: Return response view model for unexpected behavior
            throw;
        }
    }
}
