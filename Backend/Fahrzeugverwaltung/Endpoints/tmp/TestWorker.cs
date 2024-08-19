using MassTransit;

namespace Fahrzeugverwaltung.Endpoints.tmp;

public record TestBusEvent
{
    public string Value { get; set; } = string.Empty;
}

public class TestWorker // : BackgroundService
{
    private readonly IBus _bus;
    private readonly ILogger _logger;
    
    public TestWorker(IBus bus, ILogger logger)
    {
        _bus = bus;
        _logger = logger;
    }

    // protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    // {
    //     while (!stoppingToken.IsCancellationRequested)
    //     {
    //         await _bus.Publish(new TestBusEvent() {Value = $"The time is {DateTimeOffset.Now}"}, stoppingToken);
    //         _logger.Information("Worker sent message");
    //
    //         await Task.Delay(1000, stoppingToken);
    //     }
    // }
}