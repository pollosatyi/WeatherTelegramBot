namespace WeatherTelegramBot.Services.TelegramBotServices
{
    public interface ITelegramBotService
    {
        Task StartAsync();
        Task StopAsync();
    }
}
