using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using WeatherAPI.Application.Commons;

namespace WeatherAPI.Application.CustomExceptionMiddleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"Failed to fetch weather data: {ex}");
            if (ex.StatusCode == HttpStatusCode.NotFound)
            {
                await HandleNotFoundExceptionAsync(httpContext, ex);
            }
            else
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsync(new ErrorDetails()
        {
            StatusCode = context.Response.StatusCode,
            Message = "Internal Server Error from the custom middleware."
        }.ToString());
    }

    private async Task HandleNotFoundExceptionAsync(HttpContext context, HttpRequestException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        await context.Response.WriteAsync(new ErrorDetails()
        {
            StatusCode = context.Response.StatusCode,
            Message = "Weather data for the requested location is not found."
        }.ToString());
    }
}
