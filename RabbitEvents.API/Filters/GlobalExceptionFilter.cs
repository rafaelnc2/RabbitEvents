using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RabbitEvents.Shared.Responses;

namespace RabbitEvents.API.Filters;

public class GlobalExceptionFilter : Attribute, IAsyncExceptionFilter
{
    public Task OnExceptionAsync(ExceptionContext context)
    {
        var logger = context.HttpContext.RequestServices.GetService<ILogger<GlobalExceptionFilter>>()!;

        logger.LogError($"Exception - {context.Exception.StackTrace}");

        return Task.Run(() =>
        {
            var result = new ApiResponse<object>(System.Net.HttpStatusCode.InternalServerError, false, new List<string>() { context.Exception.Message });

            context.Result = new JsonResult(result)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        });
    }
}
