using AutoFixture;
using BusinessLogic.Validators.Reservation;
using DataAccess.Provider.DateTimeProvider;
using DataAccess.ReservationService;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Model.Configuration;
using Model.Reservation;
using Model.Reservation.Requests;
using Moq;

namespace BusinesslogicTests.Validators.Reservation;

public class CreateReservationValidatorLogicTest
{
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock = new();
    private readonly IFixture _fixture;

    private readonly ReservationRestrictions _noRestrictions = new()
    {
        MaxReservationDays = 0,
        MinReservationDays = 0,
        MaxReservationTimeInAdvanceInDays = 0,
        MinReservationTimeInAdvanceInDays = 0
    };

    private readonly Mock<IOptionsMonitor<Configuration>> _optionsMonitorMock = new();
    private readonly Mock<IReservationService> _reservationServiceMock = new();
    private readonly CreateReservationValidatorLogic _sut;

    public CreateReservationValidatorLogicTest()
    {
        _fixture = new Fixture();
        _optionsMonitorMock.Setup(x => x.CurrentValue).Returns(new Configuration() {ReservationRestrictions = _noRestrictions});
        _dateTimeProviderMock.Setup(x => x.DateToday).Returns(new DateOnly(2024, 05, 1));
        _sut = new(_reservationServiceMock.Object, _optionsMonitorMock.Object, _dateTimeProviderMock.Object);
    }

    [Fact]
    public async Task VehicleId_IsValid_ButIsAlreadyReserved()
    {
        var input = _fixture
            .Build<CreateReservationRequest>()
            .With(x => x.StartDateInclusive, new DateOnly(2024, 05, 10))
            .With(x => x.EndDateInclusive, new DateOnly(2024, 05, 20))
            .Create();
        _reservationServiceMock
            .Setup(x => x.GetReservationsInTimespan(input.StartDateInclusive, input.EndDateInclusive, input.Vehicle))
            .ReturnsAsync(new[]
            {
                new ReservationModel()
                {
                    StartDateInclusive = new DateOnly(2024, 05, 15),
                    EndDateInclusive = new DateOnly(2024, 05, 15)
                }
            });


        var result = await _sut.CheckIfVehicleIsAvailable(input.Vehicle, input.StartDateInclusive, input.EndDateInclusive, default);

        result.Should().BeFalse();
    }

    public static IEnumerable<object[]> GetNumbers()
    {
        DateOnly defaultStartDate = new(2024, 5, 10);
        DateOnly defaultEndDate = new(2024, 5, 20);
        DateOnly todayDateMock = new(2024, 5, 1);
        int totalDays = 11;
        ReservationRestrictions defaultReservationRestrictions = new(maxReservationDays: 90)
        {
            MaxReservationDays = 90,
            MinReservationDays = 0,
            MaxReservationTimeInAdvanceInDays = 0,
            MinReservationTimeInAdvanceInDays = 0
        };


        yield return [defaultStartDate, defaultEndDate, defaultReservationRestrictions, todayDateMock, true]; // valid

        yield return [defaultStartDate, defaultEndDate, new ReservationRestrictions(maxReservationDays: 5), todayDateMock, false]; //not valid doe tu MaxReservationDays
        yield return [defaultStartDate, defaultEndDate, new ReservationRestrictions(maxReservationDays: 11), todayDateMock, true]; //valid. 11 days reservation and 11 days limit
        yield return [defaultStartDate, defaultEndDate, new ReservationRestrictions(maxReservationDays: 10), todayDateMock, false]; //not valid. 11 days reservation and 10 days limit
        yield return [defaultStartDate, defaultEndDate, new ReservationRestrictions(), todayDateMock, true]; //valid. 11 days reservation and no days limit


        yield return [defaultStartDate, defaultEndDate, new ReservationRestrictions(minReservationDays: 30), todayDateMock, false]; //not valid doe tu minReservationDays
        yield return [defaultStartDate, defaultEndDate, new ReservationRestrictions(minReservationDays: 10), todayDateMock, true]; //valid. 11 days reservation and 10 days min day limit
        yield return [defaultStartDate, defaultEndDate, new ReservationRestrictions(minReservationDays: 11), todayDateMock, true]; //valid. 11 days reservation and 11 days min day limit
        yield return [defaultStartDate, defaultEndDate, new ReservationRestrictions(minReservationDays: 12), todayDateMock, false]; //not valid. 11 days reservation and 12 days min day limit
        yield return [defaultStartDate, defaultEndDate, new ReservationRestrictions(), todayDateMock, true]; //valid. 11 days reservation and no min day limit


        ReservationRestrictions min1DayInAdvance = new(minReservationTimeInAdvanceInDays: 1);
        ReservationRestrictions min2DaysInAdvance = new(minReservationTimeInAdvanceInDays: 2);
        yield return [defaultStartDate, defaultEndDate, new ReservationRestrictions(), new DateOnly(2024, 5, 10), true]; //No min time in advance
        yield return [defaultStartDate, defaultEndDate, min1DayInAdvance, new DateOnly(2024, 5, 10), false];
        yield return [defaultStartDate, defaultEndDate, min1DayInAdvance, new DateOnly(2024, 5, 9), true];
        yield return [defaultStartDate, defaultEndDate, min1DayInAdvance, new DateOnly(2024, 5, 8), true];
        yield return [defaultStartDate, defaultEndDate, min1DayInAdvance, new DateOnly(1975, 10, 29), true];
        yield return [defaultStartDate, defaultEndDate, min2DaysInAdvance, new DateOnly(2024, 5, 10), false];
        yield return [defaultStartDate, defaultEndDate, min2DaysInAdvance, new DateOnly(2024, 5, 9), false];
        yield return [defaultStartDate, defaultEndDate, min2DaysInAdvance, new DateOnly(2024, 5, 8), true];
        yield return [defaultStartDate, defaultEndDate, min2DaysInAdvance, new DateOnly(2024, 5, 7), true];
        yield return [defaultStartDate, defaultEndDate, min2DaysInAdvance, new DateOnly(1975, 10, 29), true];

        ReservationRestrictions max5DaysInAdvance = new(maxReservationTimeInAdvanceInDays: 5);
        yield return [defaultStartDate, defaultEndDate, new ReservationRestrictions(), new DateOnly(1975, 10, 29), true]; //No max time in advance 
        yield return [defaultStartDate, defaultEndDate, max5DaysInAdvance, new DateOnly(2024, 5, 10), true];
        yield return [defaultStartDate, defaultEndDate, max5DaysInAdvance, new DateOnly(2024, 5, 9), true];
        yield return [defaultStartDate, defaultEndDate, max5DaysInAdvance, new DateOnly(2024, 5, 8), true];
        yield return [defaultStartDate, defaultEndDate, max5DaysInAdvance, new DateOnly(2024, 5, 7), true];
        yield return [defaultStartDate, defaultEndDate, max5DaysInAdvance, new DateOnly(2024, 5, 6), true];
        yield return [defaultStartDate, defaultEndDate, max5DaysInAdvance, new DateOnly(2024, 5, 5), true];
        yield return [defaultStartDate, defaultEndDate, max5DaysInAdvance, new DateOnly(2024, 5, 4), false];
        yield return [defaultStartDate, defaultEndDate, max5DaysInAdvance, new DateOnly(2024, 5, 3), false];
    }

    [Theory]
    [MemberData(nameof(GetNumbers))]
    public async Task DatesAreForbiddenByConfigurationTests(DateOnly startDate, DateOnly endDate, ReservationRestrictions reservationRestrictions, DateOnly todayMock, bool shouldBeValid)
    {
        var input = _fixture
            .Build<CreateReservationRequest>()
            .With(x => x.StartDateInclusive, startDate)
            .With(x => x.EndDateInclusive, endDate)
            .Create();
        _optionsMonitorMock.Setup(x => x.CurrentValue)
            .Returns(new Configuration()
            {
                ReservationRestrictions = reservationRestrictions
            });
        _dateTimeProviderMock.Setup(x => x.DateToday).Returns(todayMock);

        var result = _sut.CheckReservationAgainstConfiguration(input.StartDateInclusive, input.EndDateInclusive);

        result.Should().Be(shouldBeValid);
    }
}