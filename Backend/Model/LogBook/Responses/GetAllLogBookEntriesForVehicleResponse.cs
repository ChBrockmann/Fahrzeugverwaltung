namespace Model.LogBook.Responses;

public record GetAllLogBookEntriesForVehicleResponse
{
    public List<LogBookEntryDto> LogBookEntries { get; set; } = new();
}