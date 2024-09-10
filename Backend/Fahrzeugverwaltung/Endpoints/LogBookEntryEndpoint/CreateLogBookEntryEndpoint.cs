using Contracts;
using MassTransit;

namespace Fahrzeugverwaltung.Endpoints.LogBookEntryEndpoint;

public record CreateLogBookRequest
{
    public string Message { get; set; } = string.Empty;
    public IFormFile ImageFile { get; set; } = null!;
}



public class CreateLogBookEntryEndpoint : Endpoint<CreateLogBookRequest, EmptyResponse>
{
    private readonly ILogger _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateLogBookEntryEndpoint(ILogger logger, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public override void Configure()
    {
        Post("logbookentry");
        AllowFileUploads();
    }

    public override async Task HandleAsync(CreateLogBookRequest req, CancellationToken ct)
    {
        _logger.Information("Creating log book entry...");

        using MemoryStream memoryStream = new MemoryStream();
        await req.ImageFile.CopyToAsync(memoryStream, ct);
        byte[] bytes = memoryStream.ToArray();

        LogbookEntryCreatedEvent logBookEntryCreated = new()
        {
            ImageAsBase64 = Convert.ToBase64String(bytes),
            ImageType = req.ImageFile.ContentType
        };

        await _publishEndpoint.Publish(logBookEntryCreated, ct);

        await SendOkAsync(ct);
    }
}