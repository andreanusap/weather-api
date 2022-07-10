using LazyCache;
using WeatherAPI.Application.Commons;
using WeatherAPI.Application.Interfaces;
using WeatherAPI.Application.Services;
using WeatherAPI.Domain;
using WeatherAPI.Domain.Interfaces;
using WeatherAPI.Infrastructure.CachedServices;
using WeatherAPI.Infrastructure.ExternalServices;
using WeatherAPI.Infrastructure.HttpClients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureServices(IServiceCollection services)
{
    var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

    services.Configure<ExternalApiOptions>(configuration.GetSection("ApiSettings"));

    services.AddHttpClient();
    services.AddLazyCache();
    services.AddAutoMapper(typeof(MappingProfile));

    services.AddSingleton<IHttpClientHelper, HttpClientHelper>();
    services.AddSingleton<ExternalWeatherService>();
    services.AddSingleton<IExternalWeatherService>(
        x => new CachedExternalWeatherService(x.GetRequiredService<ExternalWeatherService>(), x.GetRequiredService<IAppCache>()));

    services.AddTransient<IWeatherService, WeatherService>();
}