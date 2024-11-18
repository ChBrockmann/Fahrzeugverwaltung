using Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Model;

namespace Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Startup;

public static class StartupConfigurationExtensions
{
    public static Configuration SetupConfiguration(this WebApplicationBuilder builder)
    {
        builder
            .Configuration
            .AddJsonFile("./configuration/appsettings.json", true, true)
            .AddUserSecrets(typeof(Program).Assembly, true, true)
            .AddEnvironmentVariables();

        builder.Services.Configure<Configuration>(builder.Configuration);
        Configuration? configuration = builder.Configuration.Get<Configuration>();
        if (configuration is null) throw new Exception("Configuration is null");

        return configuration;
    }
}