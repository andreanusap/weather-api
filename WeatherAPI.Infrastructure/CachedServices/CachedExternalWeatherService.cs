using LazyCache;
using WeatherAPI.Domain.Interfaces;
using WeatherAPI.Domain.Models;

namespace WeatherAPI.Infrastructure.CachedServices;

public class CachedExternalWeatherService : IExternalWeatherService
{
    private readonly IExternalWeatherService _externalWeatherService;
    private readonly IAppCache _appCache;

    public CachedExternalWeatherService(IExternalWeatherService externalWeatherService, IAppCache appCache)
    {
        _externalWeatherService = externalWeatherService;
        _appCache = appCache;
    }

    /// <summary>
    /// Get data from the cache or add the response to cache if data is not exist
    /// </summary>
    /// <param name="cityId">City Id</param>
    /// <returns>Weather Data</returns>
    public async Task<WeatherData> Get5DaysForecastByCityId(string cityId)
    {
        return await _appCache.GetOrAddAsync<WeatherData>(cityId,
            async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3);
                return await _externalWeatherService.Get5DaysForecastByCityId(entry.Key.ToString()!);
            });
    }
}
