namespace Contracts;

public record LogbookEntryCreatedEvent
{
    public string ImageAsBase64 { get; set; } = string.Empty;
    public string ImageType { get; set; } = string.Empty;
}