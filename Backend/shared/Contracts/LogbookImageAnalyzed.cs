using Model.LogBook;

namespace Contracts;

public class LogbookImageAnalyzed
{
    public LogBookEntryId LogBookEntryId { get; set; } = LogBookEntryId.Empty;

    public int DetectedMileageInKm { get; set; }
}