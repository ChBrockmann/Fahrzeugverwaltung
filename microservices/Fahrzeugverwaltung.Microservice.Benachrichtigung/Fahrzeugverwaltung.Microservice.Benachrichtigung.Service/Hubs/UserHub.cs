using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Hubs;

[Authorize]
public class UserHub : Hub
{
    private readonly ILogger _logger;
    
    public UserHub(ILogger logger)
    {
        _logger = logger;
    }

    public override Task OnConnectedAsync()
    {
        _logger.Information("Connected");    
        return base.OnConnectedAsync();
    }
    
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}