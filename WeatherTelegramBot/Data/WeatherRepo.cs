using WeatherTelegramBot.Models;

namespace WeatherTelegramBot.Data
{
    public class WeatherRepo : IWeatherRepo
    {
        public Task CreateCityWetherAsync(WeatherModel weatherModel)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCityWeatherAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task GetWeatherAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task UpdateCityWeatherAsync(WeatherModel weatherModel, int id)
        {
            throw new NotImplementedException();
        }
    }
}
