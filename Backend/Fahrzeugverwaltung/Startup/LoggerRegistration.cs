using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Fahrzeugverwaltung.Startup;

public static class LoggerRegistration
{
    public static ILogger SetupLogger(this WebApplicationBuilder builder)
    {
        Logger logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithRequestUserId()
            // .Enrich.WithSensitiveDataMasking(configure =>
            // {
            //     configure.Mode = MaskingMode.Globally;
            // })
            .MinimumLevel.Information()
            // .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
            .WriteTo.Console(LogEventLevel.Verbose, "[{Timestamp:HH:mm:ss} {SourceContext} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File("./logs/info/info-.txt", LogEventLevel.Information,
                rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30)
            .WriteTo.File("./logs/error/error-.txt", LogEventLevel.Error,
                rollingInterval: RollingInterval.Month, retainedFileCountLimit: 12)
            .ReadFrom.AppSettings()
            .CreateLogger();

        builder.Services.AddSerilog(logger);
        return logger;
    }
}