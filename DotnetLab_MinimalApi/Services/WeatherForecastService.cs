namespace DotnetLab_MinimalApi.Services;

public interface IWeatherForecastGetService
{
    IEnumerable<WeatherForecast> Get();
}
public interface IWeatherForecastPostService
{
    Task<WeatherForecast> Post(WeatherForecast forecast);
}
public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
public class WeatherForecastGetService : IWeatherForecastGetService
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];
    
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            Summaries[Random.Shared.Next(Summaries.Length)]
        ))
        .ToArray();
    }
}
public class WeatherForecastPostService : IWeatherForecastPostService
{
    private static readonly IList<WeatherForecast> MyWeatherForecast = [];
    public Task<WeatherForecast> Post(WeatherForecast forecast)
    {
        MyWeatherForecast.Add(forecast);
        return Task.FromResult(forecast);
    }
}

