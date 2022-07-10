using AutoMapper;
using WeatherAPI.Domain.Models;
using WeatherAPI.Domain.ViewModels;

namespace WeatherAPI.Application.Commons;

public class MappingProfile : Profile
{
    /// <summary>
    /// Auto mapper profile
    /// </summary>
    public MappingProfile()
    {
        CreateMap<Weather, WeatherInformationViewModel>();
        CreateMap<City, CityViewModel>()
            .ForMember(d => d.Sunrise, o => o.MapFrom(s => DateTimeOffset.FromUnixTimeSeconds(s.Sunrise).UtcDateTime))
            .ForMember(d => d.Sunset, o => o.MapFrom(s => DateTimeOffset.FromUnixTimeSeconds(s.Sunset).UtcDateTime));
    }
}
