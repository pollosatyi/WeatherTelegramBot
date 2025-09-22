using AutoMapper;
using WeatherTelegramBot.DTOs;
using WeatherTelegramBot.Models;
using static WeatherTelegramBot.DTOs.CreateWeatherDto;


namespace WeatherTelegramBot.Profiles
{
    public class WeatherProfile : Profile
    {
        public WeatherProfile()
        {

            //Маппинг из сложного WeatherDto в простую WeatherModel
            CreateMap<WeatherDto, WeatherModel>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Temperature, opt => opt.MapFrom(src => src.Main.Temp))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Weather.First().Description))
                .ForMember(dest => dest.WindSpeed, opt => opt.MapFrom(src => src.Wind.Speed));

            // Использование


            CreateMap<CreateWeatherDto, WeatherModel>();


        }
    }
}
