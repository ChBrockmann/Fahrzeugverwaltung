using DataAccess.LogBookEntryService;
using Model.LogBook;
using Model.LogBook.Requests;
using Model.LogBook.Responses;

namespace Fahrzeugverwaltung.Endpoints.LogBookEntryEndpoint;

public class GetAllLogBookEntriesForVehicleEndpoint : Endpoint<GetAllLogBookEntiresForVehicleRequest, GetAllLogBookEntriesForVehicleResponse>
{
    private readonly IMapper _mapper;
    private readonly ILogBookEntryService _logBookEntryService;

    public GetAllLogBookEntriesForVehicleEndpoint(IMapper mapper, ILogBookEntryService logBookEntryService)
    {
        _mapper = mapper;
        _logBookEntryService = logBookEntryService;
    }

    public override void Configure()
    {
        Get("logbookentry/vehicle/{VehicleModelId}");
    }

    public override async Task HandleAsync(GetAllLogBookEntiresForVehicleRequest req, CancellationToken ct)
    {
        IEnumerable<LogBookEntry> result = await _logBookEntryService.GetAllForVehicle(req.VehicleModelId);
        IEnumerable<LogBookEntryDto> response = _mapper.Map<IEnumerable<LogBookEntryDto>>(result);

        await SendOkAsync(new GetAllLogBookEntriesForVehicleResponse
        {
            LogBookEntries = response.ToList()
        }, ct);
    }
}