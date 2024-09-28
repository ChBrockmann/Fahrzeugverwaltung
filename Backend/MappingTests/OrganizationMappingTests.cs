using AutoFixture;
using FluentAssertions;
using Model.Organization;
using Model.Organization.Responses;

namespace MappingTests;

public class OrganizationMappingTests : TestBase
{
    [Fact]
    public void Organization_To_OrganizationBasicResponse()
    {
        OrganizationModel input = Fixture.Create<OrganizationModel>();
        OrganizationBasicResponse expected = new()
        {
            Id = input.Id,
            Name = input.Name,
            Description = input.Description
        };

        OrganizationBasicResponse actual = Mapper.Map<OrganizationBasicResponse>(input);

        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void Organization_To_OrganizationAdminResponse()
    {
        OrganizationModel input = Fixture.Create<OrganizationModel>();
        OrganizationAdminResponse expected = new()
        {
            Id = input.Id,
            Name = input.Name,
            Description = input.Description,
            IsAdmin = false
        };

        OrganizationAdminResponse actual = Mapper.Map<OrganizationAdminResponse>(input);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Organization_To_OrganizationDto()
    {
        OrganizationModel input = Fixture.Create<OrganizationModel>();
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