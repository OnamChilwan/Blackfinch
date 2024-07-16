using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blackfinch.Api.ComponentTests;

public class TestStartup() : Startup(BuildConfiguration())
{
    protected override void ConfigureExternalDependencies(IServiceCollection services)
    {
    }
    
    private static IConfigurationRoot BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection()
            .Build();
    }
}