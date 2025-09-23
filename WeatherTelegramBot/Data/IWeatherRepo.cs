using WeatherTelegramBot.Models;

namespace WeatherTelegramBot.Data
{
    public interface IWeatherRepo
    {
        Task SaveChanges();
        Task GetWeatherAsync(int id);

        Task CreateCityWetherAsync(WeatherModel weatherModel);

        Task UpdateCityWeatherAsync(WeatherModel weatherModel, int id);

        Task DeleteCityWeatherAsync(int id);
    }
}
