using System.ComponentModel.DataAnnotations;

namespace WeatherTelegramBot.Models
{
    public class WeatherModel
    {
        [Key]
        public string City { get; set; }
        public double Temperature { get; set; }
        public string Description { get; set; }
        public double WindSpeed { get; set; }

        public int RequestCount { get; set; }  // Как часто запрашивали город
        public double AverageTemperature { get; set; } // Средняя температура
    }
}
