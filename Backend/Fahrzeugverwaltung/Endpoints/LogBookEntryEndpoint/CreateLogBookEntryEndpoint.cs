using Model.LogBook.Requests;

namespace Fahrzeugverwaltung.Endpoints.LogBookEntryEndpoint;

public class CreateLogBookEntryEndpoint : Endpoint<CreateLogBookRequest, EmptyResponse>
{
    private readonly ILogger _logger;
    
    public CreateLogBookEntryEndpoint(ILogger logger)
    {
        _logger = logger;
    }
}