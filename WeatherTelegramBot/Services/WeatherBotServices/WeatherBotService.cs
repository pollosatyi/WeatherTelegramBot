
using AutoMapper;
using WeatherTelegramBot.Data;
using WeatherTelegramBot.Services.TelegramBotServices;
using WeatherTelegramBot.Services.WeatherEndpointsServices;
using WeatherTelegramBot.Services.WeatherServies;

namespace WeatherTelegramBot.Services.WeatherTelegramServices
{
    public class WeatherBotService : IWeatherBotService
    {
        private IWeatherService _weatherService;
        private readonly IMapper _mapper;
        private readonly IWeatherRepo _weatherRepo;
        public WeatherBotService(IWeatherService weatherService, IMapper mapper, IWeatherRepo weatherRepo)
        {
            _weatherService = weatherService;
            _mapper = mapper;
            _weatherRepo = weatherRepo;
        }
        public async Task<string> GetWeatherMesageAsync(string cityName)
        {
            try
            {
                var weatherData = await _weatherService.GetWeatherAsync(cityName, _mapper);
                if (weatherData == null)
                    return $"❌ Не удалось получить данные для города '{cityName}'";
                _weatherRepo.CreateModel(cityName.ToLower(), weatherData);


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
