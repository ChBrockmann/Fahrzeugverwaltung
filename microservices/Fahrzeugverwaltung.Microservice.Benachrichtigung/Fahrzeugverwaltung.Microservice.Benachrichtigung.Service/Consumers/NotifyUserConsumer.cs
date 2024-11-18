using Contracts;
using MassTransit;

namespace Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Consumers;

public class NotifyUserConsumer : IConsumer<NotifyUserEvent>
{
    private readonly ILogger _logger;
    
    public NotifyUserConsumer(ILogger logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<NotifyUserEvent> context)
    {
        var user = context.Message.UserId;
        var message = context.Message.Message;
        
        _logger.Information("UserId: {UserId} Message: {Message}", user, message);

        // await context.Publish<NotifyUserEvent>(new { UserId = user, Message = message });
    }
}