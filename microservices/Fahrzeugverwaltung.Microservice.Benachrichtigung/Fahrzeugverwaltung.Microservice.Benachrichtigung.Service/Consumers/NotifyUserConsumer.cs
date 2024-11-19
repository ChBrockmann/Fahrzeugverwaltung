using Contracts;
using Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Hubs;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Consumers;

public class NotifyUserConsumer : IConsumer<NotifyUserEvent>
{
    private readonly ILogger _logger;
    private readonly IHubContext<UserHub> _userHub;
    
    public NotifyUserConsumer(ILogger logger, IHubContext<UserHub> userHub)
    {
        _logger = logger;
        _userHub = userHub;
    }

    public async Task Consume(ConsumeContext<NotifyUserEvent> context)
    {
        var user = context.Message.UserId;
        var message = context.Message.Message;
        
        _logger.Information("UserId: {UserId} Message: {Message}", user, message);

        await _userHub.Clients.All.SendAsync("TestSendMessage", message );
    }
}