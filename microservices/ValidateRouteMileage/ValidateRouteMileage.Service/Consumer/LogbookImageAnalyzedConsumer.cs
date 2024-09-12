using Contracts;
using MassTransit;

namespace ValidateRouteMileage.Service.Consumer;

public class LogbookImageAnalyzedConsumer : IConsumer<LogbookImageAnalyzed>
{
    public async Task Consume(ConsumeContext<LogbookImageAnalyzed> context)
    {
        Console.WriteLine("Received: {0}", context.Message.DetectedTotalMileage);
    }
}