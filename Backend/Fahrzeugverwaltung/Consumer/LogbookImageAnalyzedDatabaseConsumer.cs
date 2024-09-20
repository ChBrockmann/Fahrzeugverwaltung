using Contracts;
using DataAccess.LogBookEntryService;
using MassTransit;
using Model.LogBook;

namespace Fahrzeugverwaltung.Consumer;

public class LogbookImageAnalyzedDatabaseConsumer : IConsumer<LogbookImageAnalyzed>
{
    private readonly ILogBookEntryService _logBookEntryService;
    private readonly ILogger _logger;

    public LogbookImageAnalyzedDatabaseConsumer(ILogBookEntryService logBookEntryService, ILogger logger)
    {
        _logBookEntryService = logBookEntryService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<LogbookImageAnalyzed> context)
    {
        _logger.Information("Received {TotalMileage} for Logbook ID {LogBookEntryId}", context.Message.DetectedMileageInKm, context.Message.LogBookEntryId);

        LogBookEntry? result = await _logBookEntryService.SetEndMileage(context.Message.LogBookEntryId, context.Message.DetectedMileageInKm);

        if (result is null) _logger.Error("Could not find Logbook Entry {LogBookEntryId}", context.Message.LogBookEntryId);
    }
}