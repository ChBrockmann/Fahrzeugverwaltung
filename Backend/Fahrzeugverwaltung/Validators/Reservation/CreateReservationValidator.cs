﻿using BusinessLogic.Validators.Reservation;
using BusinessLogic.Validators.Vehicle;
using FluentValidation;
using Model.Reservation.Requests;

namespace Fahrzeugverwaltung.Validators.Reservation;

public class CreateReservationValidator : AbstractValidator<CreateReservationRequest>
{
    public CreateReservationValidator(CreateReservationValidatorLogic reservationValidator, VehicleValidator vehicleValidator)
    {
        RuleFor(x => x.StartDateInclusive)
            .LessThanOrEqualTo(x => x.EndDateInclusive)
            .WithName("startDateBeforeEnddate")
            .WithMessage("Startdate has to be before Enddate")
            .Must((request, startDate) => reservationValidator.CheckMaxReservationDays(request.StartDateInclusive, request.EndDateInclusive))
            .WithMessage("Reservation exceeds maximum reservation days")
            .Must((request, startDate) => reservationValidator.CheckMinReservationDays(request.StartDateInclusive, request.EndDateInclusive))
            .WithMessage("Reservation is below minimum reservation days")
            .Must(reservationValidator.CheckMinReservationTimeInAdvance)
            .WithName("minReservationTimeInAdvance")
            .WithMessage("Reservation is below minimum reservation time in advance")
            .Must(reservationValidator.CheckMaxReservationTimeInAdvance)
            .WithMessage("Reservation is above maximum reservation time in advance");

        RuleFor(x => x.Vehicle)
            .MustAsync(vehicleValidator.CheckIfVehicleExists)
            .WithMessage("Vehicle does not exist")
            .MustAsync(async (request, vehicleId, ct) =>
                await reservationValidator.CheckIfVehicleIsAvailable(vehicleId, request.StartDateInclusive, request.EndDateInclusive, ct))
            .WithMessage("Vehicle is not available in the given time frame");
    }
}