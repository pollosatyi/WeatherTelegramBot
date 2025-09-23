using AutoMapper;
using WeatherTelegramBot.Models;

namespace WeatherTelegramBot.Services
{
    public interface IWeatherService
    {
        Task<WeatherModel> GetWeatherAsync(string cityName, IMapper mapper);
    }
}
