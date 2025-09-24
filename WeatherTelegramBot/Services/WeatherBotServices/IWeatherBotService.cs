namespace WeatherTelegramBot.Services.WeatherTelegramServices
{
    public interface IWeatherBotService
    {
        Task<string> GetWeatherMesageAsync(string cityName);
    }
}
