using Model.Vehicle;
using AutoFixture;
using FluentAssertions;

namespace MappingTests;

public class VehicleMappingTests : TestBase
{
    [Fact]
    public void VehicleModel_To_VehicleModelDto()
    {
        VehicleModel input = Fixture.Create<VehicleModel>();
        VehicleModelDto expected = new()
        {
            Id = input.Id,
            Name = input.Name
        };

        VehicleModelDto actual = Mapper.Map<VehicleModelDto>(input);

        actual.Should().BeEquivalentTo(expected);
    }
}