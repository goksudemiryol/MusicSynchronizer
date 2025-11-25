using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Mime;

namespace MusicSynchronizer.Api.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    //private readonly ILoggerService _loggerService;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
        //_loggerService = loggerService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            //Continue request.
            await _next(context);
        }
        catch (Exception ex)
        {
            //_loggerService.Log(ex.Message);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = MediaTypeNames.Text.Plain;

            await context.Response.WriteAsync($"An unexpected error occurred.\n{ex.Message}\n{ex.InnerException?.Message}");

            return;
        }
    }
}
