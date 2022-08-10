using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using WeatherAPI.Application.Interfaces;
using WeatherAPI.Domain.ViewModels;

namespace WeatherAPI.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;
    private readonly ILogger<WeatherController> _logger;

    public WeatherController(IWeatherService weatherService, ILogger<WeatherController> logger)
    {
        _weatherService = weatherService;
        _logger = logger;
    }

    /// <summary>
    /// Get 5 days forecast by location or city id
    /// </summary>
    /// <param name="locationId"></param>
    /// <returns>Weather Data View Model</returns>
    [HttpGet("locations/{locationId}")]
    public async Task<ActionResult<WeatherDataViewModel>> GetByLocation(string locationId)
    {
        try
        {
            var result = await _weatherService.GetWeatherByLocation(locationId);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        catch (HttpRequestException ex)
        {
            

            _logger.LogError(ex.Message);
            _logger.LogError(ex.StackTrace);

            var inner = ex.InnerException;
            while (inner is not null)
            {
                _logger.LogError(inner.StackTrace);
                inner = inner.InnerException;
            }

            if (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            else
            {
                return StatusCode(500, "Could not fetch data from the weather service. Please try again later.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _logger.LogError(ex.StackTrace);
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Get summary of user's favorite temperatures & locations
    /// </summary>
    /// <param name="unit">The unit in celsius or fahrenheit</param>
    /// <param name="temperature">The temperature</param>
    /// <param name="locations">List of locations in comma separated string</param>
    /// <returns>A list of Weather Data View Model</returns>
    [HttpGet("summary")]
    public async Task<ActionResult<IEnumerable<WeatherDataViewModel>>> GetWeatherSummaries([FromQuery] string unit, [FromQuery] double temperature, [FromQuery] string locations)
    {
        try
        {
            var unitStrings = new[] { "celsius", "fahrenheit" };

            if (string.IsNullOrWhiteSpace(unit)
                || !unitStrings.Contains(unit)
                || string.IsNullOrWhiteSpace(locations))
            {
                return BadRequest();
            }

            var result = await _weatherService.GetWeatherSummary(unit, temperature, locations);

            if (result is null)
            {

                return NotFound();
            }

            return Ok(result);
        }
        catch (InvalidCastException invalidEx)
        {
            return StatusCode(400, invalidEx.Message);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex.Message);
            _logger.LogError(ex.StackTrace);

            var inner = ex.InnerException;
            while (inner is not null)
            {
                _logger.LogError(inner.StackTrace);
                inner = inner.InnerException;
            }

            if (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            else
            {
                return StatusCode(500, "Could not fetch data from the weather service. Please try again later.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _logger.LogError(ex.StackTrace);
            return StatusCode(500);
        }
    }
}
