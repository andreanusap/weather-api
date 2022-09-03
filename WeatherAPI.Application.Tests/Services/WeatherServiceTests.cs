using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherAPI.Application.Commons;
using WeatherAPI.Application.Interfaces;
using WeatherAPI.Application.Services;
using WeatherAPI.Domain.Interfaces;
using WeatherAPI.Domain.Models;
using WeatherAPI.Domain.ViewModels;
using Xunit;

namespace WeatherAPI.Application.Tests.Services;

public class WeatherServiceTests
{
    static readonly Fixture Fixture = new Fixture();

    private IWeatherService _weatherService;
    private Mock<IMapper> mockMapper = new Mock<IMapper>();
    private Mock<IExternalWeatherService> mockExternalWeatherService = new Mock<IExternalWeatherService>();

    [Fact]
    public async Task GetWeatherByLocation_ShouldReturnNull_WhenExternalWeatherServiceReturnsNull()
    {
        //arrange
        WeatherData mockWeather = null;
        mockExternalWeatherService
            .Setup(s => s.Get5DaysForecastByCityId(It.IsAny<string>()))
            .ReturnsAsync(mockWeather);

        _weatherService = new WeatherService(mockExternalWeatherService.Object, mockMapper.Object);

        //act
        var result = await _weatherService.GetWeatherByLocation("mockId");

        //assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetWeatherByLocation_ShouldReturnWeatherData_WhenLocationIdIsValid()
    {
        //arrange
        var locationId = 12345;
        var city = Fixture.Build<City>()
            .With(x => x.Id, locationId)
            .Create();
        var weatherData = Fixture.Build<WeatherData>()
            .With(x => x.City, city)
            .Create();

        mockExternalWeatherService
            .Setup(s => s.Get5DaysForecastByCityId(locationId.ToString()))
            .ReturnsAsync(weatherData);

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        var mapper = mockMapper.CreateMapper();

        _weatherService = new WeatherService(mockExternalWeatherService.Object, mapper);

        //act
        var result = await _weatherService.GetWeatherByLocation(locationId.ToString());

        //assert
        result.Should().BeOfType<WeatherDataViewModel>();
        result.City.Id.Should().Be(locationId);
    }

    [Fact]
    public async Task GetWeatherSummary_ShouldReturnCelsiusWeatherData_WhenUnitRequestIsCelsius()
    {
        //arrange
        var temperature = 20;
        var date = DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds();
        var mainDetail1 = Fixture.Build<MainDetail>()
            .With(x => x.Temp, temperature + 273.15)
            .Create();
        var mainDetail2 = Fixture.Build<MainDetail>()
            .With(x => x.Temp, temperature - 5 + 273.15)
            .Create();
        var listDetail1 = Fixture.Build<ListDetail>()
            .With(x => x.Date, date)
            .With(x => x.Main, mainDetail1)
            .Create();
        var listDetail2 = Fixture.Build<ListDetail>()
            .With(x => x.Date, date)
            .With(x => x.Main, mainDetail2)
            .Create();
        var listDetails = new List<ListDetail>();
        listDetails.Add(listDetail1);
        listDetails.Add(listDetail2);
        var weatherData = Fixture.Build<WeatherData>()
            .With(x => x.List, listDetails)
            .Create();

        mockExternalWeatherService
            .Setup(s => s.Get5DaysForecastByCityId(It.IsAny<string>()))
            .ReturnsAsync(weatherData);

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        var mapper = mockMapper.CreateMapper();

        _weatherService = new WeatherService(mockExternalWeatherService.Object, mapper);

        //act
        var result = await _weatherService.GetWeatherSummary("celsius", temperature, "12345");

        //assert
        result.Should().BeOfType<List<WeatherDataViewModel>>();
        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetWeatherSummary_ShouldReturnFahrenheitWeatherData_WhenUnitRequestIsFahrenheit()
    {
        //arrange
        var temperature = 70;
        var date = DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds();
        var mainDetail1 = Fixture.Build<MainDetail>()
            .With(x => x.Temp, (temperature - 32) * 5 / 9 + 273.15)
            .Create();
        var mainDetail2 = Fixture.Build<MainDetail>()
            .With(x => x.Temp, (temperature - 10 - 32) * 5 / 9 + 273.15)
            .Create();
        var listDetail1 = Fixture.Build<ListDetail>()
            .With(x => x.Date, date)
            .With(x => x.Main, mainDetail1)
            .Create();
        var listDetail2 = Fixture.Build<ListDetail>()
            .With(x => x.Date, date)
            .With(x => x.Main, mainDetail2)
            .Create();
        var listDetails = new List<ListDetail>();
        listDetails.Add(listDetail1);
        listDetails.Add(listDetail2);
        var weatherData = Fixture.Build<WeatherData>()
            .With(x => x.List, listDetails)
            .Create();

        mockExternalWeatherService
            .Setup(s => s.Get5DaysForecastByCityId(It.IsAny<string>()))
            .ReturnsAsync(weatherData);

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        var mapper = mockMapper.CreateMapper();

        _weatherService = new WeatherService(mockExternalWeatherService.Object, mapper);

        //act
        var result = await _weatherService.GetWeatherSummary("fahrenheit", temperature, "12345");

        //assert
        result.Should().BeOfType<List<WeatherDataViewModel>>();
        result.Should().HaveCount(1);
    }
}
