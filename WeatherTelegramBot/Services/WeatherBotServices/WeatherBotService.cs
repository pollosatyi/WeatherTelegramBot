
using AutoMapper;
using WeatherTelegramBot.Services.TelegramBotServices;
using WeatherTelegramBot.Services.WeatherServies;

namespace WeatherTelegramBot.Services.WeatherTelegramServices
{
    public class WeatherBotService : IWeatherBotService
    {
        private IWeatherService _weatherService;
        private readonly IMapper _mapper;
        public WeatherBotService(IWeatherService weatherService, IMapper mapper)
        {
            _weatherService = weatherService;
            _mapper = mapper;
        }
        public async Task<string> GetWeatherMesageAsync(string cityName)
        {
            try
            {
                var weatherData = await _weatherService.GetWeatherAsync(cityName, _mapper);
                if (weatherData == null)
                    return $"❌ Не удалось получить данные для города '{cityName}'";


                return $"🌤️ Погода в {weatherData.City}:\n\n" +
                       $"🌡️ Температура: {weatherData.Temperature}K\n" +
                       $"📝 Описание: {weatherData.Description}\n" +
                       $"💨 Скорость ветра: {weatherData.WindSpeed} м/с\n\n" +
                       $"Введите другой город для поиска:";

            }
            catch (Exception ex)
            {
                return $"❌ Ошибка: {ex.Message}";

            }

        }
    }
}
