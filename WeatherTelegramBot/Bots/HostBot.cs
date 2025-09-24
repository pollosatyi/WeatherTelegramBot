using AutoMapper;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WeatherTelegramBot.Bots
{
    public class HostBot
    {
        public event Action<ITelegramBotClient, Update> OnMessage;
        private TelegramBotClient _bot;
        private long _chatId;
       
        public HostBot(string token, long chatId)
        {

            _bot = new TelegramBotClient(token);
            _chatId = chatId;
           
        }

        public void Start()
        {
            _bot.StartReceiving(UpdateHandler, ErrorHandler);

            Console.WriteLine("бот запушен");
        }

        public void Stop()
        {
            _bot.CloseAsync();
        }

        public async Task SendMessageAsync(long chatId, string text)
        {
            await _bot.SendTextMessageAsync(chatId, text);
        }

        public async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken token)
        {
            Console.WriteLine($"Пришло сообщение: {update.Message?.Text ?? "не текст"}\n");
            OnMessage?.Invoke(client, update);
            await Task.CompletedTask;
        }

        private async Task ErrorHandler(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            Console.WriteLine("Ошибка: " + exception.Message);
            await Task.CompletedTask;
        }

        public async Task  SendFirstMessage()
        {
            
               await Task.Delay(100);
               await _bot.SendTextMessageAsync(_chatId, "Бот готов к работе.\n Введите название города:\n");
            

        }
    }
}
