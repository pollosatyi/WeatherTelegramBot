
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
using WeatherTelegramBot.Services.WeatherServies;
using WeatherTelegramBot.Services.TelegramBotServices;
using WeatherTelegramBot.Services.WeatherTelegramServices;
using WeatherTelegramBot.Services.WeatherEndpointsServices;
using WeatherTelegramBot.Services.BackGroudServices;

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
            builder.Services.AddScoped<ITelegramBotService, TelegramBotService>();
            builder.Services.AddScoped<IWeatherBotService, WeatherBotService>();
            builder.Services.AddScoped<TelegramBotbackGroundService>();
            

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

            app.MapWeatherEndPoints();
            //var scope = app.Services.CreateScope();

            //    var botService = scope.ServiceProvider.GetRequiredService<ITelegramBotService>();
            //    botService.StartAsync();



            //app.MapGet("api/v1/weather/{city}", async (string cityName, IMapper mapper, IWeatherService weatherService) =>
            //{
            //    var weatherModel = await weatherService.GetWeatherAsync(cityName, mapper);
            //    if (weatherModel == null) return Results.Problem("Ошибка");
            //    return Results.Ok(weatherModel);
            //});

            //HostBot bot = new HostBot("8019909456:AAFXV9Z4-FQXuqCb3djqQk6Djf4DlNs6IeI");
            //bot.Start();
            //bot.OnMessage+=bot.OnMessage;

            //Task.Run(async () =>
            //{
            //    await Task.Delay(100);
            //    await bot.SendMessageAsync(8019909456, "Бот готов к работе.\n Введите название города:\n");
            //}
            //);

            using (var scope = app.Services.CreateScope())
            {
                var botService = scope.ServiceProvider.GetRequiredService<TelegramBotbackGroundService>();
                _ = Task.Run(() => botService.StartAsync(default)); // Если есть свой метод запуска
            }

            app.Run();
        }
        //private static async void OnMessage(ITelegramBotClient client, Update update, IMapper? mapper = default)
        //{
        //    if (update.Message == null || update.Message?.Text == null) return;
        //    using var scope = app.Services.CreateScope();
        //    var weatherService = scope.ServiceProvider.GetRequiredService<WeatherService>();

        //    var messageText = update.Message?.Text;
        //    var chatId = update.Message?.Chat.Id ?? 0;

        //    if (update.Message?.Text == "/start")
        //    {
        //        await client.SendTextMessageAsync(chatId, "\"🌤️ Добро пожаловать в Weather Bot!\\n\\n\" +\r\n            \"Просто отправьте мне название города, и я покажу погоду.\\n\\n\" +\r\n            \"Пример:\\n\\\"Москва\\\"\\n\\\"London\\\"\\n\\\"Paris\\\"\"");
        //    }
        //    else
        //    {
        //        try
        //        {
        //            await client.SendChatActionAsync(chatId, ChatAction.Typing);

        //            var cityName = messageText?.Trim() ?? "";
        //            var weatherInfo = await GetWeatherForCity(cityName, mapper);

        //            await client.SendTextMessageAsync(chatId, weatherInfo);


        //        }
        //        catch (Exception ex)
        //        {
        //            await client.SendTextMessageAsync(chatId,
        //               $"❌ Ошибка при получении погоды для города '{messageText}': {ex.Message}");


        //        }
        //    }

        //}

        //private static async Task<string> GetWeatherForCity(string cityName, IMapper? mapper, IWeatherService weatherService)
        //{
        //    try
        //    {

        //        var weatherData =  await weatherService.GetWeatherAsync(cityName,mapper);
        //        var weatherDataModel =weatherData;


        //        if (weatherDataModel == null)
        //        {
        //            return $"❌ Не удалось получить данные для города '{cityName}'";
        //        }

        //        return $"🌤️ Погода в {weatherDataModel.City}:\n\n" +
        //       $"🌡️ Температура: {weatherDataModel.Temperature}°C\n" +
        //       $"📝 Описание: {weatherDataModel.Description}\n" +
        //       $"💨 Скорость ветра: {weatherDataModel.WindSpeed} м/с\n\n" +
        //       $"Введите другой город для поиска:";

        //    }
        //    catch (Exception ex)
        //    {
        //        return $"❌ Ошибка: {ex.Message}";


        //    }
        //}
    }
}
