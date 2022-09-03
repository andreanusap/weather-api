using Microsoft.AspNetCore.Builder;
using WeatherAPI.Application.CustomExceptionMiddleware;

namespace WeatherAPI.Application.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}
