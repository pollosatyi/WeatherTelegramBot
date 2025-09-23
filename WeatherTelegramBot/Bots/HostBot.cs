using AutoMapper;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WeatherTelegramBot.Bots
{
    public class HostBot
    {
        public Action<ITelegramBotClient, Update, IMapper?>? OnMessage;
        private TelegramBotClient _bot;
       
        public HostBot(string token)
        {

            _bot = new TelegramBotClient(token);
           
        }

        public void Start()
        {
            _bot.StartReceiving(UpdateHandler, ErrorHandler);

            Console.WriteLine("бот запушен");
        }

        public async Task SendMessageAsync(long chatId, string text)
        {
            await _bot.SendTextMessageAsync(chatId, text);
        }

        private async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken token)
        {
            Console.WriteLine($"Пришло сообщение: {update.Message?.Text ?? "не текст"}\n");
            OnMessage?.Invoke(client, update, default);
            await Task.CompletedTask;
        }

        private async Task ErrorHandler(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            Console.WriteLine("Ошибка: " + exception.Message);
            await Task.CompletedTask;
        }

        
    }
}
