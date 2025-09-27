using AutoMapper;
using System.Text.Json;
using WeatherTelegramBot.DTOs;
using WeatherTelegramBot.Models;

namespace WeatherTelegramBot.Services.WeatherServies
{
    public class WeatherService : IWeatherService
    {
        const string API_KEY = "5d539df439fbce04ed7bfa0a2516e38f";
        const string URLpart1 = "https://api.openweathermap.org/data/2.5/weather?q=";
        const string URLpart2 = "&appid=";

        public async Task<WeatherModel> GetWeatherAsync(string cityName, IMapper mapper)
        {
            try
            {
                string url = $"{URLpart1}{cityName}{URLpart2}{API_KEY}";
                using var client = new HttpClient();
                var response = await client.GetAsync(url);



                var json = await response.Content.ReadAsStringAsync();
                var weatherDataDto = JsonSerializer.Deserialize<CreateModelDto>(json, new JsonSerializerOptions
                {

                    PropertyNameCaseInsensitive = true
                });

                var weatherDataModel = mapper?.Map<WeatherModel>(weatherDataDto);

                return weatherDataModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting weather for {cityName}: {ex.Message}");
                return null;


            }
        }
    }
}
