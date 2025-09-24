
using AutoMapper;
using WeatherTelegramBot.Data;
using WeatherTelegramBot.DTOs;
using WeatherTelegramBot.Services.WeatherServies;

namespace WeatherTelegramBot.Services.WeatherEndpointsServices
{
    public static class WeatherEndpoints
    {
        public static void MapWeatherEndPoints(this IEndpointRouteBuilder app)
        {
            var groupFromOpenWeather = app.MapGroup("api/v1/weatherFromOpenWeatherMap");

            groupFromOpenWeather.MapGet("/get/{city}", GetWeatherFromOpenWeather);
            groupFromOpenWeather.MapPost("/post/{city}", CreateWeatherFromOpenWeather);
            groupFromOpenWeather.MapDelete("/delete/{city}", DeleteWeatherFromOpenWeather);

        }

        private static async Task<IResult> DeleteWeatherFromOpenWeather(string cityName, IWeatherRepo weatherRepo)
        {
            return await weatherRepo.DeleteWeatherModelAsync(cityName.ToLower());
        }

        private static async Task<IResult> CreateWeatherFromOpenWeather(string cityName,
            IWeatherService weatherService,
            IMapper mapper,
            IWeatherRepo weatherRepo)
        {
            try
            {
                var weatherModel = await weatherService.GetWeatherAsync(cityName.ToLower(), mapper);

                if (weatherModel == null) return Results.NotFound($"Погода для города '{cityName}' не найдена");

                await weatherRepo.CreateWetherModelAsync(weatherModel);



                return Results.Ok(mapper.Map<ReadModelDto>(weatherModel));

            }
            catch (Exception ex)
            {

                return Results.Problem($"Ошибка: {ex.Message}");
            }


        }



        private static async Task<IResult> GetWeatherFromOpenWeather(string cityName, IWeatherService weatherService, IMapper mapper)
        {
            try
            {
                var weatherModel = await weatherService.GetWeatherAsync(cityName.ToLower(), mapper);

                if (weatherModel == null) return Results.NotFound($"Погода для города '{cityName}' не найдена");

                return Results.Ok(mapper.Map<ReadModelDto>(weatherModel));

            }
            catch (Exception ex)
            {

                return Results.Problem($"Ошибка: {ex.Message}");
            }
        }
    }
}
