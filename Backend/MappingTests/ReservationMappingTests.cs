using AutoFixture;
using FluentAssertions;
using Model.Reservation;
using Model.Reservation.Requests;
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
            ReservationCreated = input.ReservationCreated,
            VehicleReserved = Mapper.Map<VehicleModelDto>(input.VehicleReserved),
            ReservationMadeByUser = Mapper.Map<UserDto>(input.ReservationMadeByUser)
        };

        ReservationModelDto actual = Mapper.Map<ReservationModelDto>(input);

        actual.Should().BeEquivalentTo(expected);
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
            ReservationCreated = DateTime.Now,
            VehicleReserved = new VehicleModel() {Id = VehicleModelId.Empty},
            ReservationMadeByUser = new UserModel() {Id = input.ReservedBy.ToString()}
        };

        ReservationModel actual = Mapper.Map<ReservationModel>(input);

        actual.Should().BeEquivalentTo(expected, opt => opt
            .Excluding(o => o.ReservationCreated)
            .Excluding(o => o.ReservationMadeByUser)
            .Excluding(o => o.ReservationMadeByUser.SecurityStamp)
            .Excluding(o => o.ReservationMadeByUser.ConcurrencyStamp)
        );
    }
}