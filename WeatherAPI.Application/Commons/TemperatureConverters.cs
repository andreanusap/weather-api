namespace WeatherAPI.Application.Commons;

public static class TemperatureConverters
{
    public static double CelsiusToKelvin(double temperature)
    {
        return temperature + 273.15;
    }

    public static double FahrenheitToKelvin(double temperature)
    {
        return Math.Round((temperature - 32) * 5 / 9 + 273.15, 2);
    }

    public static double KelvinToCelcius(double temperature)
    {
        return Math.Round(temperature - 273.15, 2);
    }

    public static double KelvinToFahrenheit(double temperature)
    {
        return Math.Round(1.8 * (temperature - 273.15) + 32, 2);
    }
}
