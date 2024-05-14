﻿using System.ComponentModel.DataAnnotations;
using Model.Vehicle;

namespace Model.Reservation.Requests;

public record CheckAvailabilityForVehicleAndTimespanRequest
{
    [Required]
    public VehicleModelId RequestedVehicleId { get; set; }
    
    [Required]
    public DateOnly StartDateInclusive { get; set; }
    
    [Required]
    public DateOnly EndDateInclusive { get; set; }
}