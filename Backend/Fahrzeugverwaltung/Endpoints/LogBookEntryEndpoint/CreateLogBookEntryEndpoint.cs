using MassTransit;

namespace Fahrzeugverwaltung.Endpoints.LogBookEntryEndpoint;

public record CreateLogBookRequest
{
    public string Message { get; set; } = string.Empty;
    public IFormFile ImageFile { get; set; } = null!;
}

public record TestEvent
{
    public string Message { get; set; } = string.Empty;
    public string ImageFile { get; set; } = null!;
}

public class CreateLogBookEntryEndpoint : Endpoint<CreateLogBookRequest, EmptyResponse>
{
    private readonly ILogger _logger;
    private readonly IBus _bus;

    public CreateLogBookEntryEndpoint(ILogger logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }

    public override void Configure()
    {
        Post("logbookentry");
        AllowFileUploads();
    }

    public override async Task HandleAsync(CreateLogBookRequest req, CancellationToken ct)
    {
        _logger.Information("Creating log book entry...");

        StreamReader reader = new(req.ImageFile.OpenReadStream());
        string text = await reader.ReadToEndAsync(ct);

        TestEvent testEvent = new()
        {
            Message = req.Message,
            ImageFile = text
        };

        await _bus.Publish(testEvent, ct);

        await SendOkAsync(ct);
    }
}