using AutoMapper;
using WeatherTelegramBot.Models;

namespace WeatherTelegramBot.Services.WeatherServies
{
    public interface IWeatherService
    {
        Task<WeatherModel> GetWeatherAsync(string cityName, IMapper mapper);
    }
}
