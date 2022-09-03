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
    private readonly ExternalApiOptions _options;

    public HttpClientHelper(HttpClient httpClient, IOptions<ExternalApiOptions> options)
    {
        _options = options.Value;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri($"{_options.BaseUrl}");
    }

    /// <summary>
    /// Method helper for Http Get request
    /// </summary>
    /// <param name="uri">The Uri of the request</param>
    /// <returns>Weather Data</returns>
    public async Task<WeatherData> HttpGet(string uri)
    {
        var request = $"{uri}&appid={_options.ApiKey}";
        try
        {
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, request);
            using var httpResponse = await _httpClient.SendAsync(httpRequest);

            httpResponse.EnsureSuccessStatusCode();

            var content = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<WeatherData>(content);
        } 
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException($"Http Request to {request} returns {ex.StatusCode}", ex);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
