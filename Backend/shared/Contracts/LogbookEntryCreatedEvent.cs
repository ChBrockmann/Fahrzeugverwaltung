using Model.LogBook;

namespace Contracts;

public class LogbookEntryCreatedEvent
{
    public LogBookEntryId LogBookEntryId { get; set; } = LogBookEntryId.Empty;
    public string ImageAsBase64 { get; set; } = string.Empty;
    public string ImageType { get; set; } = string.Empty;
}