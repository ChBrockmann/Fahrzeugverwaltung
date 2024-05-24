using AutoFixture;
using DataAccess.ReservationService;
using DataAccess.VehicleService;
using Fahrzeugverwaltung.Provider.DateTimeProvider;
using Fahrzeugverwaltung.Validators.Reservation;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Options;
using Model.Configuration;
using Model.Reservation;
using Model.Reservation.Requests;
using Model.Vehicle;
using Moq;

namespace BusinesslogicTests.Validators.Reservation;

public class CreateReservationValidatorTest
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
    private readonly CreateReservationValidator _sut;
    private readonly Mock<IVehicleService> _vehicleServiceMock = new();

    public CreateReservationValidatorTest()
    {
        _fixture = new Fixture();
        _optionsMonitorMock.Setup(x => x.CurrentValue).Returns(new Configuration() {ReservationRestrictions = _noRestrictions});
        _vehicleServiceMock.Setup(x => x.Exists(It.IsAny<VehicleModelId>())).ReturnsAsync(true);
        _dateTimeProviderMock.Setup(x => x.DateToday).Returns(new DateOnly(2024, 05, 1));
        _sut = new(_vehicleServiceMock.Object, _reservationServiceMock.Object, _optionsMonitorMock.Object, _dateTimeProviderMock.Object);
    }

    [Fact]
    public async Task Startdate_HasToBeBefore_Enddate_NotFullfiled()
    {
        var model = new CreateReservationRequest()
        {
            StartDateInclusive = new DateOnly(2024, 05, 20),
            EndDateInclusive = new DateOnly(2024, 05, 10),
            Vehicle = new VehicleModelId()
        };
        _vehicleServiceMock
            .Setup(x => x.Exists(It.IsAny<VehicleModelId>()))
            .ReturnsAsync(true);


        TestValidationResult<CreateReservationRequest>? result = await _sut.TestValidateAsync(model);


        result.ShouldHaveValidationErrorFor(x => x.StartDateInclusive).Only();
    }

    [Fact]
    public async Task Startdate_HasToBeBefore_Enddate_Fullfiled()
    {
        var model = new CreateReservationRequest()
        {
            StartDateInclusive = new DateOnly(2024, 05, 10),
            EndDateInclusive = new DateOnly(2024, 05, 20),
            Vehicle = new VehicleModelId()
        };


        TestValidationResult<CreateReservationRequest>? result = await _sut.TestValidateAsync(model);


        result.ShouldNotHaveValidationErrorFor(x => x.StartDateInclusive);
        result.ShouldNotHaveValidationErrorFor(x => x.EndDateInclusive);
    }

    [Fact]
    public async Task VehicleId_IsInvalid()
    {
        var input = _fixture
            .Build<CreateReservationRequest>()
            .With(x => x.StartDateInclusive, new DateOnly(2024, 05, 10))
            .With(x => x.EndDateInclusive, new DateOnly(2024, 05, 20))
            .Create();
        _vehicleServiceMock
            .Setup(x => x.Exists(It.IsAny<VehicleModelId>()))
            .ReturnsAsync(false);


        var result = await _sut.TestValidateAsync(input);


        result.ShouldHaveValidationErrorFor(x => x.Vehicle).Only();
    }

    [Fact]
    public async Task VehicleId_IsValid_ButIsAlreadyReserved()
    {
        var input = _fixture
            .Build<CreateReservationRequest>()
            .With(x => x.StartDateInclusive, new DateOnly(2024, 05, 10))
            .With(x => x.EndDateInclusive, new DateOnly(2024, 05, 20))
            .Create();
        _vehicleServiceMock
            .Setup(x => x.Exists(It.IsAny<VehicleModelId>()))
            .ReturnsAsync(true);
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


        var result = await _sut.TestValidateAsync(input);

        result.ShouldHaveValidationErrorFor(x => x.Vehicle).Only();
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
        _vehicleServiceMock
            .Setup(x => x.Exists(It.IsAny<VehicleModelId>()))
            .ReturnsAsync(true);
        _optionsMonitorMock.Setup(x => x.CurrentValue)
            .Returns(new Configuration()
            {
                ReservationRestrictions = reservationRestrictions
            });
        _dateTimeProviderMock.Setup(x => x.DateToday).Returns(todayMock);

        var result = await _sut.TestValidateAsync(input);

        if (shouldBeValid)
        {
            result.ShouldNotHaveValidationErrorFor(x => x.StartDateInclusive);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(x => x.StartDateInclusive);
        }
    }
}