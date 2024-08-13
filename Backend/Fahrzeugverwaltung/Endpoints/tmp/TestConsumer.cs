using MassTransit;

namespace Fahrzeugverwaltung.Endpoints.tmp;

public class TestConsumer : IConsumer<TestBusEvent>
{
    private readonly ILogger _logger;
    public TestConsumer(ILogger logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TestBusEvent> context)
    {
        _logger.Information("Received: {Value}", context.Message.Value);
        await Task.CompletedTask;
    }
}