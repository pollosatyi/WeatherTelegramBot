
using AutoMapper;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WeatherTelegramBot.Bots;
using WeatherTelegramBot.Services.WeatherTelegramServices;

namespace WeatherTelegramBot.Services.TelegramBotServices
{
    public class TelegramBotService : ITelegramBotService
    {
        private readonly HostBot _bot;
        private readonly IWeatherBotService _weatherBotService;
        private readonly ILogger<TelegramBotService> _logger;
        private readonly string _token = "8019909456:AAFXV9Z4-FQXuqCb3djqQk6Djf4DlNs6IeI";
        private readonly long _chatIdBot = 8019909456;

        public TelegramBotService(IWeatherBotService weatherBotService, ILogger<TelegramBotService> logger)
        {
            _bot = new HostBot(_token, _chatIdBot);
            _weatherBotService = weatherBotService;
            _logger = logger;

            _bot.OnMessage += OnMessageReceived;


        }
        public async Task StartAsync()
        {
            _bot.Start();
            _logger.LogInformation("Телеграм запушен");
            await _bot.SendFirstMessage();


            await Task.CompletedTask;
        }

        public async Task StopAsync()
        {
            _bot.Stop();
            await Task.CompletedTask;
        }

        private async void OnMessageReceived(ITelegramBotClient client, Update update)
        {
            if (update.Message == null || update.Message.Text == null) return;
            var messageText = update.Message.Text;
            var chatId = update.Message.Chat.Id;

            try
            {
                if (messageText == "/start")
                {
                    await client.SendTextMessageAsync(chatId,
                        "🌤️ Добро пожаловать в Weather Bot!\n\n" +
                        "Просто отправьте мне название города, и я покажу погоду.\n\n" +
                        "Пример:\n\"Москва\"\n\"London\"\n\"Paris\"");
                }
                else
                {
                    await client.SendChatActionAsync(chatId, ChatAction.Typing);
                    var weatherMessage = await _weatherBotService.GetWeatherMesageAsync(messageText.Trim());
                    await client.SendTextMessageAsync(chatId, weatherMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Telegram message");
                await client.SendTextMessageAsync(chatId,
                    $"❌ Ошибка при обработке запроса: {ex.Message}");
            }

        }


    }
}
