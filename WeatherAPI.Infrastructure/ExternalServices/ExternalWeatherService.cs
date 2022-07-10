using Microsoft.Extensions.Logging;
using System.Net;
using WeatherAPI.Domain.Interfaces;
using WeatherAPI.Domain.Models;

namespace WeatherAPI.Infrastructure.ExternalServices;

public class ExternalWeatherService : IExternalWeatherService
{
    private readonly IHttpClientHelper _httpClientHelper;
    private readonly ILogger<ExternalWeatherService> _logger;

    public ExternalWeatherService(IHttpClientHelper httpClientHelper, ILogger<ExternalWeatherService> logger)
    {
        _httpClientHelper = httpClientHelper;
        _logger = logger;
    }

    /// <summary>
    /// Get 5 days forecasts by city id
    /// </summary>
    /// <param name="cityId">City Id</param>
    /// <returns>Weather Data</returns>
    public async Task<WeatherData> Get5DaysForecastByCityId(string cityId)
    {
        try
        {
            return await _httpClientHelper.HttpGet($"forecast?id={cityId}");
        }
        catch (HttpRequestException httpReqEx)
        {
            var message = "";
            switch (httpReqEx.StatusCode)
            {
                case HttpStatusCode.TooManyRequests:
                    message = "Limit rate exceeded";
                    _logger.LogError(httpReqEx, message);
                    throw new Exception(message);
                case HttpStatusCode.NotFound:
                    message = $"Weather for city id {cityId} is not found";
                    _logger.LogError(httpReqEx, message);
                    return null;
                case HttpStatusCode.Unauthorized:
                    message = "Api Key provided is invalid";
                    _logger.LogError(httpReqEx, message);
                    throw new Exception("Unable to fetch weather data");
                default:
                    _logger.LogError(httpReqEx, "Failed to get weather data from external API");
                    throw new Exception("Unable to fetch weather data");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get the 5 Days Forecast By City Id");
            throw ex;
        }
    }
}