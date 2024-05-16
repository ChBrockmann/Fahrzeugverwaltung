﻿using AutoFixture;
using FluentAssertions;
using Model.Reservation;
using Model.Reservation.Requests;
using Model.ReservationStatus;
using Model.User;
using Model.Vehicle;

namespace MappingTests;

public class ReservationMappingTests : TestBase
{
    [Fact]
    public void ReservationModel_To_ReservationModelDto()
    {
        ReservationModel input = Fixture.Create<ReservationModel>();
        ReservationModelDto expected = new()
        {
            Id = input.Id,
            EndDateInclusive = input.EndDateInclusive,
            StartDateInclusive = input.StartDateInclusive,
            VehicleReserved = Mapper.Map<VehicleModelDto>(input.VehicleReserved),
            ReservationStatus = Mapper.Map<List<ReservationStatusModelDto>>(input.ReservationStatus),
        };

        ReservationModelDto actual = Mapper.Map<ReservationModelDto>(input);
        
        actual.Should().BeEquivalentTo(expected, opt => opt
            .Excluding(x => x.ReservationCreated)
            .Excluding(x => x.ReservationMadeByUser));
    }


    [Fact]
    public void CreateReservationRequest_To_ReservationModel()
    {
        CreateReservationRequest input = Fixture.Create<CreateReservationRequest>();
        ReservationModel expected = new()
        {
            Id = ReservationId.Empty,
            StartDateInclusive = input.StartDateInclusive,
            EndDateInclusive = input.EndDateInclusive,
            VehicleReserved = new VehicleModel() {Id = VehicleModelId.Empty},
            ReservationStatus = new(),
        };

        ReservationModel actual = Mapper.Map<ReservationModel>(input);

        actual.Should().BeEquivalentTo(expected, opt => opt)
        ;
    }
}