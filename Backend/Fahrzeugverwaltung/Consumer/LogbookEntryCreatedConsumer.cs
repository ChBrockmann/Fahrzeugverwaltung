using Contracts;
using MassTransit;

namespace Fahrzeugverwaltung.Consumer;

public class LogbookEntryCreatedConsumer : IConsumer<LogbookEntryCreatedEvent>
{
    public async Task Consume(ConsumeContext<LogbookEntryCreatedEvent> context)
    {
        await Task.CompletedTask;
        //Discrad Method. Onyl used to create the exchange and queue automatically
    }
}