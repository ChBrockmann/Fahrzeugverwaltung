using AutoFixture;
using FluentAssertions;
using Model.LogBook;
using Model.Reservation;
using Model.User;
using Model.Vehicle;

namespace MappingTests;

public class LogBookEntryMappingTests : TestBase
{
    [Fact]
    public void LogBookEntry_To_LogBookEntryDto()
    {
        LogBookEntry input = Fixture.Create<LogBookEntry>();
        LogBookEntryDto expected = new()
        {
            Id = input.Id,
            Description = input.Description,
            CreatedBy = Mapper.Map<UserDto>(input.CreatedBy),
            AssociatedVehicle = Mapper.Map<VehicleModelDto>(input.AssociatedVehicle),
            AssociatedReservation = input.AssociatedReservation is null ? null : Mapper.Map<ReservationModelDto>(input.AssociatedReservation),
            CreatedAt = input.CreatedAt,
            EndMileageInKm = input.EndMileageInKm
        };

        LogBookEntryDto actual = Mapper.Map<LogBookEntryDto>(input);

        actual.Should().BeEquivalentTo(expected);
    }
}