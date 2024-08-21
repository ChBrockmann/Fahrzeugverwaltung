using AutoFixture;
using FluentAssertions;
using Model.Organization;

namespace MappingTests;

public class OrganizationMappingTests : TestBase
{
    [Fact]
    public void Organization_To_OrganizationResponse()
    {
        Organization input = Fixture.Create<Organization>();
        OrganizationDto expected = new()
        {
            Id = input.Id,
            Name = input.Name,
            Description = input.Description
        };

        OrganizationDto actual = Mapper.Map<OrganizationDto>(input);

        actual.Should().BeEquivalentTo(expected);
    }
}