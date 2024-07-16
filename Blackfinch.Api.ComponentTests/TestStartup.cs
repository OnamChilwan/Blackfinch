using Blackfinch.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Blackfinch.Api.ComponentTests;

public class TestStartup() : Startup(BuildConfiguration())
{
    protected override void ConfigureExternalDependencies(IServiceCollection services)
    {
        services.AddSingleton<IDomainRepository>(_ => Substitute.For<IDomainRepository>());
    }
    
    private static IConfigurationRoot BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection()
            .Build();
    }
}