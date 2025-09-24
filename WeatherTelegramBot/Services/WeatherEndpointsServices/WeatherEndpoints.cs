
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

            groupFromOpenWeather.MapGet("/get/{city.ToLower}", GetWeatherFromOpenWeather);
            groupFromOpenWeather.MapPost("/post/{city.ToLower}", CreateWeatherFromOpenWeather);

        }

        private static async Task<IResult> CreateWeatherFromOpenWeather(string cityName,
            IWeatherService weatherService,
            IMapper mapper,
            IWeatherRepo weatherRepo)
        {
            try
            {
                var weatherModel = await weatherService.GetWeatherAsync(cityName, mapper);

                if (weatherModel == null) return Results.NotFound($"Погода для города '{cityName}' не найдена");

                var findCityFromDb = await weatherRepo.GetWeatherModelAsync(cityName);
                if(findCityFromDb == null)
                {
                    await weatherRepo.CreateWetherModelAsync(weatherModel);
                }
                else
                {
                    await weatherRepo.UpdateWeatherModelAsync(weatherModel,findCityFromDb);
                }

                
             
                return Results.Ok(mapper.Map<ReadModelDto>(weatherModel));

            }
            catch (Exception ex)
            {

                return Results.Problem($"Ошибка: {ex.Message}");
            }


        }

        //private static async Task<ReadModelDto?> GetCityFromDb(string cityName, IWeatherRepo weatherRepo)
        //{
        //    var weatherModel = await weatherRepo.GetWeatherModelAsync(cityName);

        //}

        private static async Task<IResult> GetWeatherFromOpenWeather(string cityName, IWeatherService weatherService, IMapper mapper)
        {
            try
            {
                var weatherModel = await weatherService.GetWeatherAsync(cityName, mapper);

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
