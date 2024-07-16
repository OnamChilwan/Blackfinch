using Blackfinch.Api.Validators;
using FluentValidation;

namespace Blackfinch.Api;

public class Startup(IConfiguration configuration)
{
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddApplicationPart(typeof(Startup).Assembly);
        services.AddRouting();
        services.AddValidatorsFromAssemblyContaining<LoanRequestValidator>();
        
        ConfigureExternalDependencies(services);
    }

    protected virtual void ConfigureExternalDependencies(IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }
}