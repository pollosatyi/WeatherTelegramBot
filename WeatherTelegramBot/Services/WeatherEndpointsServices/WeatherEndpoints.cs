
using AutoMapper;
using WeatherTelegramBot.Services.WeatherServies;

namespace WeatherTelegramBot.Services.WeatherEndpointsServices
{
    public static class WeatherEndpoints
    {
        public static void MapWeatherEndPoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/v1/weather");

            group.MapGet("{city}", GetWeather);
        }

        private static async Task<IResult> GetWeather(string cityName, IWeatherService weatherService, IMapper mapper)
        {
            try
            {
                var weatherModel = await weatherService.GetWeatherAsync(cityName, mapper);

                if(weatherModel == null) return Results.NotFound($"Погода для города '{cityName}' не найдена");

                return  Results.Ok(weatherModel);

            }
            catch (Exception ex)
            {

                return Results.Problem($"Ошибка: {ex.Message}");
            }
        }
    }
}
