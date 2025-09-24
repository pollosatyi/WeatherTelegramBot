using Microsoft.Extensions.Hosting;
using WeatherTelegramBot.Services.TelegramBotServices;

namespace WeatherTelegramBot.Services.BackGroudServices
{
    public class TelegramBotbackGroundService : BackgroundService
    {
        private readonly ITelegramBotService _botService;
        private readonly ILogger<TelegramBotbackGroundService> _logger;

        public TelegramBotbackGroundService(ITelegramBotService botService, ILogger<TelegramBotbackGroundService> logger)
        {
            _botService = botService;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("executeAsync работает");
            await _botService.StartAsync();
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
