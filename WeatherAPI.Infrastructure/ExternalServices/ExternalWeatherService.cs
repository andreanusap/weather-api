using Microsoft.Extensions.Logging;
using System.Net;
using WeatherAPI.Domain.Interfaces;
using WeatherAPI.Domain.Models;

namespace WeatherAPI.Infrastructure.ExternalServices;

public class ExternalWeatherService : IExternalWeatherService
{
    private readonly IHttpClientHelper _httpClientHelper;

    public ExternalWeatherService(IHttpClientHelper httpClientHelper)
    {
        _httpClientHelper = httpClientHelper;
    }

    /// <summary>
    /// Get 5 days forecasts by city id
    /// </summary>
    /// <param name="cityId">City Id</param>
    /// <returns>Weather Data</returns>
    public async Task<WeatherData> Get5DaysForecastByCityId(string cityId)
    {
        return await _httpClientHelper.HttpGet($"forecast?id={cityId}");
    }
}