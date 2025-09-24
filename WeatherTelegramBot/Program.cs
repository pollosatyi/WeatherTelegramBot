
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
using Microsoft.EntityFrameworkCore;
using Npgsql;

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

            var connectionString = builder.Configuration.GetConnectionString("PostgresDbConnection");
            
            var postgresConnectionString = new NpgsqlConnectionStringBuilder(connectionString);
            
            postgresConnectionString.Password = builder.Configuration["Password"];
            builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseNpgsql(postgresConnectionString.ToString()));

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

            // Program.cs - в самом конце, перед app.Run()
            var scope = app.Services.CreateScope(); // БЕЗ using!
            var botService = scope.ServiceProvider.GetRequiredService<TelegramBotbackGroundService>();

            // Запускаем сервис и ждем завершения приложения
            var botTask = botService.StartAsync(app.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping);
            //using (var scope = app.Services.CreateScope())
            //{
            //    var botService = scope.ServiceProvider.GetRequiredService<TelegramBotbackGroundService>();
            //    _ = Task.Run(() => botService.StartAsync(default)); // Если есть свой метод запуска
            //}

            app.Run();
        }

    }
}
