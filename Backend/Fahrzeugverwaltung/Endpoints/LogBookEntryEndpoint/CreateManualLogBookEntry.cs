﻿using DataAccess.LogBookEntryService;
using DataAccess.UserService;
using DataAccess.VehicleService;
using Model.LogBook;
using Model.LogBook.Requests;
using Model.User;
using Model.Vehicle;

namespace Fahrzeugverwaltung.Endpoints.LogBookEntryEndpoint;

public class CreateManualLogBookEntry : BaseEndpoint<CreateManualLogBookEntryRequest, LogBookEntryDto>
{
    private readonly ILogBookEntryService _logBookEntryService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IUserService _userService;
    private readonly IVehicleService _vehicleService;

    public CreateManualLogBookEntry(ILogBookEntryService logBookEntryService, IMapper mapper, ILogger logger, IUserService userService, IVehicleService vehicleService)
    {
        _logBookEntryService = logBookEntryService;
        _mapper = mapper;
        _logger = logger;
        _userService = userService;
        _vehicleService = vehicleService;
    }

    public override void Configure()
    {
        Post("logbookentry/vehicle/{VehicleModelId}");
    }

    public override async Task HandleAsync(CreateManualLogBookEntryRequest req, CancellationToken ct)
    {
        UserModel requestingUser = UserFromContext;
        if (requestingUser == null) throw new ArgumentNullException(nameof(requestingUser));

        VehicleModel? associatedVehicle = await _vehicleService.Get(req.VehicleModelId);
        if (associatedVehicle is null)
        {
            ThrowError("Vehicle not found");
            return;
        }

        LogBookEntry logBookEntry = new LogBookEntry
        {
            Id = LogBookEntryId.New(),
            EndMileageInKm = req.TotalMileageInKm,
            AssociatedVehicle = associatedVehicle,
            Description = req.Description,
            AssociatedReservation = null,
            CreatedAt = DateTime.Now,
            CreatedBy = requestingUser
        };

        await SendOkAsync(_mapper.Map<LogBookEntryDto>(await _logBookEntryService.Create(logBookEntry)), ct);
    }
}