namespace WeatherAPI.Domain.ViewModels;

public class WeatherDataViewModel
{
    public CityViewModel City { get; set; }
    public IEnumerable<MainWeatherViewModel> Weathers { get; set; }
}