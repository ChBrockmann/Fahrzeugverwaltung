﻿using Model.Reservation;
using Model.User;
using Model.Vehicle;

namespace Model.LogBook;

public record LogBookEntryDto
{
    public LogBookEntryId Id { get; set; } = LogBookEntryId.Empty;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public UserDto CreatedBy { get; set; } = new();

    public int? EndMileageInKm { get; set; }

    public ReservationModelDto? AssociatedReservation { get; set; } = new();
    public VehicleModelDto AssociatedVehicle { get; set; } = new();
}