using FluentAssertions;
using Model.Reservation;
using Model.ReservationStatus;
using Model.User;
using AutoFixture;

namespace MappingTests;

public class ReservationStatusModelTests : TestBase
{
    [Fact]
    public void ReservationStatusModel_To_ReservationStatusModelDto()
    {
        ReservationStatusModel input = Fixture.Create<ReservationStatusModel>();
        ReservationStatusModelDto expected = new()
        {
            Id = input.Id,
            Status = input.Status,
            StatusChanged = input.StatusChanged,
            StatusChangedByUser = Mapper.Map<UserDto>(input.StatusChangedByUser),
            StatusReason = input.StatusReason,
            Reservation = Mapper.Map<ReservationModelDto>(input.Reservation),
        };

        ReservationStatusModelDto actual = Mapper.Map<ReservationStatusModelDto>(input);

        actual.Should().BeEquivalentTo(expected);
    }
}