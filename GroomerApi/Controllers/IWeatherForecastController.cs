namespace GroomerApi.Controllers;

public interface IWeatherForecastController
{
    IEnumerable<WeatherForecast> Get();
}