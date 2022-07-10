namespace WeatherAPI.Domain.ViewModels;

public class CityViewModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Country { get; set; }
    public DateTime Sunrise { get; set; }
    public DateTime Sunset { get; set; }
}
