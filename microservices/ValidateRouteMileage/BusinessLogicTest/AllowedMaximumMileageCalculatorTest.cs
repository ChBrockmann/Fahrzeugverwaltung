using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using ValidateRouteMileage.BusinessLogic;
using ValidateRouteMileage.Model.Configuration;
using ValidateRouteMileage.Model.Route;

namespace BusinessLogicTest;

public class AllowedMaximumMileageCalculatorTest
{
    private readonly AllowedMaximumMileageCalculator _sut;
    private Mock<IOptionsMonitor<Configuration>> _configMock;
    private readonly Fixture _fixture = new();

    public AllowedMaximumMileageCalculatorTest()
    {
        _configMock = new();
        _sut = new AllowedMaximumMileageCalculator(_configMock.Object);
    }
    
    [Fact]
    public void Calculate_SingleDayTrip_NoAdditionalMileageAdded()
    {
        //Arrange
        _configMock.Setup(x => x.CurrentValue).Returns(new Configuration()
        {
            RouteValidationParameters = new()
            {
                GrantAdditionalDistanceAfterDays = 10,
                AllowedDeviationPercent = 0,
                AdditionalDistancePerDayInKm = 0
            }
        });
        DateOnly startDateInclusive = new(2024, 01, 01);
        DateOnly endDateInclusive = new(2024, 01, 01);
        RouteDistanceResult input = _fixture.Create<RouteDistanceResult>();

        //Act
        var actual = _sut.Calculate(startDateInclusive, endDateInclusive, input);
        
        //Assert
        actual.Should().Be(input.TotalDistanceInMeter);
    }
    
    [Fact]
    public void Calculate_SingleDayTrip_With10PercentDeviation()
    {
        //Arrange
        _configMock.Setup(x => x.CurrentValue).Returns(new Configuration()
        {
            RouteValidationParameters = new()
            {
                GrantAdditionalDistanceAfterDays = 10,
                AllowedDeviationPercent = 10,
                AdditionalDistancePerDayInKm = 0
            }
        });
        DateOnly startDateInclusive = new(2024, 01, 01);
        DateOnly endDateInclusive = new(2024, 01, 01);
        RouteDistanceResult input = _fixture
            .Build<RouteDistanceResult>()
            .With(x => x.TotalDistanceInMeter, 1000)
            .Create();

        //Act
        var actual = _sut.Calculate(startDateInclusive, endDateInclusive, input);
        
        //Assert
        actual.Should().Be(1100, "10% deviation should be added to the total distance 1000(meter) * 1.1 = 1100(meter)");
    }
    
    [Fact]
    public void Calculate_SingleDayTrip_With9PercentDeviationAndRoundDown()
    {
        //Arrange
        _configMock.Setup(x => x.CurrentValue).Returns(new Configuration()
        {
            RouteValidationParameters = new()
            {
                GrantAdditionalDistanceAfterDays = 10,
                AllowedDeviationPercent = 9,
                AdditionalDistancePerDayInKm = 0
            }
        });
        DateOnly startDateInclusive = new(2024, 01, 01);
        DateOnly endDateInclusive = new(2024, 01, 01);
        RouteDistanceResult input = _fixture
            .Build<RouteDistanceResult>()
            .With(x => x.TotalDistanceInMeter, 1337)
            .Create();

        //Act
        var actual = _sut.Calculate(startDateInclusive, endDateInclusive, input);
        
        //Assert
        actual.Should().Be(1457, "9% deviation should be added to the total distance 1337(meter) * 1.09 = 1457.33 -> 1457(meter)");
    }
    
    [Fact]
    public void Calculate_SingleDayTrip_With8PercentDeviationAndRoundUp()
    {
        //Arrange
        _configMock.Setup(x => x.CurrentValue).Returns(new Configuration()
        {
            RouteValidationParameters = new()
            {
                GrantAdditionalDistanceAfterDays = 10,
                AllowedDeviationPercent = 8,
                AdditionalDistancePerDayInKm = 0
            }
        });
        DateOnly startDateInclusive = new(2024, 01, 01);
        DateOnly endDateInclusive = new(2024, 01, 01);
        RouteDistanceResult input = _fixture
            .Build<RouteDistanceResult>()
            .With(x => x.TotalDistanceInMeter, 1337)
            .Create();

        //Act
        var actual = _sut.Calculate(startDateInclusive, endDateInclusive, input);
        
        //Assert
        actual.Should().Be(1444, "8% deviation should be added to the total distance 1337(meter) * 1.08 = 1443.96 -> 1444(meter)");
    }
    
    [Fact]
    public void Calculate_MultiDayTrip_WithFixedAllowancePerDay()
    {
        //Arrange
        _configMock.Setup(x => x.CurrentValue).Returns(new Configuration()
        {
            RouteValidationParameters = new()
            {
                GrantAdditionalDistanceAfterDays = 3,
                AllowedDeviationPercent = 0,
                AdditionalDistancePerDayInKm = 100
            }
        });
        DateOnly startDateInclusive = new(2024, 01, 01);
        DateOnly endDateInclusive = new(2024, 01, 03);
        RouteDistanceResult input = _fixture
            .Build<RouteDistanceResult>()
            .With(x => x.TotalDistanceInMeter, 1000)
            .Create();

        //Act
        var actual = _sut.Calculate(startDateInclusive, endDateInclusive, input);
        
        //Assert
        actual.Should().Be(1300);
    }
    
    [Fact]
    public void Calculate_MultiDayTrip_WithoutFixedAllowancePerDayAndDeviation()
    {
        //Arrange
        _configMock.Setup(x => x.CurrentValue).Returns(new Configuration()
        {
            RouteValidationParameters = new()
            {
                GrantAdditionalDistanceAfterDays = 4,
                AllowedDeviationPercent = 10,
                AdditionalDistancePerDayInKm = 100
            }
        });
        DateOnly startDateInclusive = new(2024, 01, 01);
        DateOnly endDateInclusive = new(2024, 01, 03);
        RouteDistanceResult input = _fixture
            .Build<RouteDistanceResult>()
            .With(x => x.TotalDistanceInMeter, 1000)
            .Create();

        //Act
        var actual = _sut.Calculate(startDateInclusive, endDateInclusive, input);
        
        //Assert
        actual.Should().Be(1100);
    }
    
    [Fact]
    public void Calculate_MultiDayTrip_WithFixedAllowancePerDayAndDeviation()
    {
        //Arrange
        _configMock.Setup(x => x.CurrentValue).Returns(new Configuration()
        {
            RouteValidationParameters = new()
            {
                GrantAdditionalDistanceAfterDays = 3,
                AllowedDeviationPercent = 10,
                AdditionalDistancePerDayInKm = 100
            }
        });
        DateOnly startDateInclusive = new(2024, 01, 01);
        DateOnly endDateInclusive = new(2024, 01, 03);
        RouteDistanceResult input = _fixture
            .Build<RouteDistanceResult>()
            .With(x => x.TotalDistanceInMeter, 1000)
            .Create();

        //Act
        var actual = _sut.Calculate(startDateInclusive, endDateInclusive, input);
        
        //Assert
        actual.Should().Be(1430, "1000 base + 100 * 3 days = 1300 * 1.1 = 1430");
    }
}