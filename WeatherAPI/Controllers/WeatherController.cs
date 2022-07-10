using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Application.Interfaces;
using WeatherAPI.Domain.ViewModels;

namespace WeatherAPI.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
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
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
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
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
