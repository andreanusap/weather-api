using Microsoft.Extensions.Logging;
using WeatherAPI.Domain.Interfaces;
using WeatherAPI.Domain.Models;
using System.Text.Json;
using WeatherAPI.Domain;
using Microsoft.Extensions.Options;

namespace WeatherAPI.Infrastructure.HttpClients;

public class HttpClientHelper : IHttpClientHelper
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HttpClientHelper> _logger;
    private readonly ExternalApiOptions _options;

    public HttpClientHelper(HttpClient httpClient, ILogger<HttpClientHelper> logger, IOptions<ExternalApiOptions> options)
    {
        _options = options.Value;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri($"{_options.BaseUrl}");
        _logger = logger;
    }

    /// <summary>
    /// Method helper for Http Get request
    /// </summary>
    /// <param name="uri">The Uri of the request</param>
    /// <returns>Weather Data</returns>
    public async Task<WeatherData> HttpGet(string uri)
    {
        try
        {
            var request = $"{uri}&appid={_options.ApiKey}";

            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, request);
            using var httpResponse = await _httpClient.SendAsync(httpRequest);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Http Client response with error code", null,statusCode: httpResponse.StatusCode);
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<WeatherData>(content);
        }
        catch (HttpRequestException httpReqEx)
        {
            var message = $"Http Get for {uri} failed with {httpReqEx.StatusCode} code";
            _logger.LogError(httpReqEx, message);
            throw httpReqEx;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Http Get failed: {ex.Message}");
            throw ex;
        }
    }
}
