using DotnetLab_MinimalApi.Services;

namespace DotnetLab_MinimalApi.Endpoints;

public static class WeatherEnpoints
{
    public static void AddWeatherEndpoints(this WebApplication app)
    {
        app.MapGet("minimal-api/weatherforecast", (IWeatherForecastGetService service) => service.Get().ToArray());
        app.MapPost("minimal-api/weatherforecast", (IWeatherForecastPostService service, WeatherForecast forecast) => service.Post(forecast));
    }
}