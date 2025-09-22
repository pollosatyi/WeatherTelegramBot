namespace WeatherTelegramBot.DTOs
{
    public class CreateWeatherDto
    {
        public class WeatherDto
        {
            public Coord Coord { get; set; }           // Географические координаты (долгота, широта)
            public List<Weather> Weather { get; set; } // Список погодных условий
            public string Base { get; set; }           // Источник данных (обычно "stations")
            public Main Main { get; set; }             // Основные метеорологические параметры
            public int Visibility { get; set; }        // Видимость в метрах
            public Wind Wind { get; set; }             // Параметры ветра
            public Clouds Clouds { get; set; }         // Облачность
            public long Dt { get; set; }               // Время расчета данных (Unix timestamp)
            public Sys Sys { get; set; }               // Системные параметры (страна, восход/закат)
            public int Timezone { get; set; }          // Смещение времени от UTC в секундах
            public int Id { get; set; }                // ID города
            public string Name { get; set; }           // Название города
            public int Cod { get; set; }               // Код ответа HTTP (200 = успех)
        }

        public class Coord
        {
            public double Lon { get; set; }            // Долгота
            public double Lat { get; set; }            // Широта
        }

        public class Weather
        {
            public int Id { get; set; }                // ID погодного условия
            public string Main { get; set; }           // Группа параметров погоды (Rain, Snow, Clouds и т.д.)
            public string Description { get; set; }    // Описание погоды на человеческом языке
            public string Icon { get; set; }           // Иконка погоды
        }

        public class Main
        {
            public double Temp { get; set; }           // Температура в Кельвинах
            public double FeelsLike { get; set; }      // Ощущаемая температура в Кельвинах
            public double TempMin { get; set; }        // Минимальная температура в Кельвинах
            public double TempMax { get; set; }        // Максимальная температура в Кельвинах
            public int Pressure { get; set; }          // Атмосферное давление в гектопаскалях (hPa)
            public int Humidity { get; set; }          // Влажность в процентах
            public int SeaLevel { get; set; }          // Давление на уровне моря в hPa
            public int GrndLevel { get; set; }         // Давление на уровне земли в hPa
        }

        public class Wind
        {
            public double Speed { get; set; }          // Скорость ветра в м/с
            public int Deg { get; set; }               // Направление ветра в градусах (метеорологические)
            public double Gust { get; set; }           // Порывы ветра в м/с
        }

        public class Clouds
        {
            public int All { get; set; }               // Облачность в процентах
        }

        public class Sys
        {
            public int Type { get; set; }              // Тип системы (внутренний параметр)
            public int Id { get; set; }                // ID системы (внутренний параметр)
            public string Country { get; set; }        // Код страны (GB, US, RU и т.д.)
            public long Sunrise { get; set; }          // Время восхода солнца (Unix timestamp)
            public long Sunset { get; set; }           // Время заката солнца (Unix timestamp)
        }
    }
}
