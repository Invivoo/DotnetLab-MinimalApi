using DotnetLab_MinimalApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotnetLab_MinimalApi.Controllers;

[ApiController]
[Route("controller/[controller]")]
public class WeatherForecastController() : ControllerBase
{
    [HttpGet]
    public IEnumerable<WeatherForecast> Get([FromServices] IWeatherForecastGetService getService) => getService.Get().ToArray();
    [HttpPost]
    public async Task<WeatherForecast> Post([FromServices] IWeatherForecastPostService postService, [FromBody] WeatherForecast forecast) => await postService.Post(forecast);
}    
