
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using WeatherTelegramBot.Profiles;
using WeatherTelegramBot.Bots;
using Telegram.Bot;
using static System.Net.WebRequestMethods;
using AutoMapper;
using WeatherTelegramBot.Models;
using System.Text.Json;
using WeatherTelegramBot.DTOs;
using WeatherTelegramBot.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Telegram.Bot.Types.Enums;
using WeatherTelegramBot.Services;

namespace WeatherTelegramBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(WeatherProfile));
            builder.Services.AddScoped<IWeatherRepo, WeatherRepo>();
            builder.Services.AddScoped<IWeatherService, WeatherService>();
            builder.Services.AddHttpClient();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();



            //using var scope = app.Services.CreateScope();

            //var httpClientFactory = scope.ServiceProvider.GetService<IHttpClientFactory>();


            const string API_KEY = "5d539df439fbce04ed7bfa0a2516e38f";
            const string URLpart1 = "https://api.openweathermap.org/data/2.5/weather?q=";
            const string URLpart2 = "&appid=";

            app.MapGet("api/v1/weather/{city}", async (string cityName, IMapper mapper, IWeatherService weatherService) =>
            {
                var weatherModel = await weatherService.GetWeatherAsync(cityName, mapper);
                if (weatherModel == null) return Results.Problem("Ошибка");
                return Results.Ok(weatherModel);

                //string url = $"{URLpart1}{city}{URLpart2}{API_KEY}";
               

                //using var client = httpClientFactory.CreateClient();


                //try
                //{
                //    var response = await client.GetAsync(url);

                //    if (!response.IsSuccessStatusCode)
                //    {
                //        return Results.Problem($"Ошибка Api: {response.StatusCode}");
                //    }

                //    var json = await response.Content.ReadAsStringAsync();
                //    var modelDto = JsonSerializer.Deserialize<CreateWeatherDto>(json);
                //    var model = mapper.Map<WeatherModel>(modelDto);
                //    return Results.Ok(model);


                //}
                //catch (Exception ex)
                //{
                //    return Results.Problem($"Ошибка: {ex.Message}");
                //}
            });

            HostBot bot = new HostBot("8019909456:AAFXV9Z4-FQXuqCb3djqQk6Djf4DlNs6IeI");
            bot.Start();
            bot.OnMessage += OnMessage;

            Task.Run(async () =>
            {
                await Task.Delay(100);
                await bot.SendMessageAsync(8019909456, "Бот готов к работе.\n Введите название города:\n");
            }
            );


            app.Run();
        }
        private static async void OnMessage(ITelegramBotClient client, Update update, IMapper? mapper = default)
        {
            if (update.Message == null || update.Message?.Text == null) return;

            var messageText = update.Message?.Text;
            var chatId = update.Message?.Chat.Id ?? 0;

            if (update.Message?.Text == "/start")
            {
                await client.SendTextMessageAsync(chatId, "\"🌤️ Добро пожаловать в Weather Bot!\\n\\n\" +\r\n            \"Просто отправьте мне название города, и я покажу погоду.\\n\\n\" +\r\n            \"Пример:\\n\\\"Москва\\\"\\n\\\"London\\\"\\n\\\"Paris\\\"\"");
            }
            else
            {
                try
                {
                    await client.SendChatActionAsync(chatId, ChatAction.Typing);

                    var cityName = messageText?.Trim() ?? "";
                    var weatherInfo = await GetWeatherForCity(cityName, mapper);

                    await client.SendTextMessageAsync(chatId, weatherInfo);


                }
                catch (Exception ex)
                {
                    await client.SendTextMessageAsync(chatId,
                       $"❌ Ошибка при получении погоды для города '{messageText}': {ex.Message}");


                }
            }

        }

        private static async Task<string> GetWeatherForCity(string cityName, IMapper? mapper)
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync($"https://localhost:7067/api/v1/weather/{cityName}");


                if (!response.IsSuccessStatusCode)//здесь получается false
                {
                    return $"❌ Город '{cityName}' не найден или ошибка сервиса";

                }

                var json = await response.Content.ReadAsStringAsync();
                var weatherDataDto = JsonSerializer.Deserialize<CreateWeatherDto>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var weatherDataModel = mapper?.Map<WeatherModel>(weatherDataDto);

                if (weatherDataModel == null)
                {
                    return $"❌ Не удалось получить данные для города '{cityName}'";
                }

                return $"🌤️ Погода в {weatherDataModel.City}:\n\n" +
               $"🌡️ Температура: {weatherDataModel.Temperature}°C\n" +
               $"📝 Описание: {weatherDataModel.Description}\n" +
               $"💨 Скорость ветра: {weatherDataModel.WindSpeed} м/с\n\n" +
               $"Введите другой город для поиска:";

            }
            catch (Exception ex)
            {
                return $"❌ Ошибка: {ex.Message}";


            }
        }
    }
}
