namespace WeatherAPI.Domain.ViewModels;

public class MainWeatherViewModel
{
    public DateTime Date { get; set; }
    public double Temperature { get; set; }
    public double FeelsLike { get; set; }
    public double MinTemperature { get; set; }
    public double MaxTemperature { get; set; }
    public double Humidity { get; set; }
    public IEnumerable<WeatherInformationViewModel> WeatherInfos { get; set; }
    public string? DateText { get; set; }
}
