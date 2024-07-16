using Blackfinch.Api;
using Microsoft.AspNetCore;

var config = CreateConfiguration().Build();

// Log.Logger = new LoggerConfiguration()
//     .MinimumLevel.Information()
//     .Enrich.FromLogContext()
//     .Enrich.WithExceptionDetails()
//     .WriteTo.Console(LogEventLevel.Information)
//     .CreateLogger();

var builder = WebHost
    .CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.AddLogging(b => b.AddConsole());
        // services.AddLogging(builder => builder.AddSerilog());
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    })
    .ConfigureAppConfiguration(app =>
    {
        app.AddConfiguration(config);
    })
    .UseStartup<Startup>();

await builder.Build().RunAsync();
return;

static IConfigurationBuilder CreateConfiguration()
{
    return new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables();
}