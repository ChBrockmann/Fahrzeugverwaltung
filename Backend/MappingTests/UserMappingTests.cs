using AutoFixture;
using FluentAssertions;
using Model.Organization;
using Model.User;

namespace MappingTests;

public class UserMappingTests : TestBase
{
    [Fact]
    public void UserModel_To_UserDto()
    {
        UserModel input = Fixture.Create<UserModel>();
        UserDto expected = new()
        {
            Id = input.Id,
            Firstname = input.Firstname,
            Lastname = input.Lastname,
            Organization = Mapper.Map<OrganizationDto>(input.Organization),
            PhoneNumber = input.PhoneNumber
        };

        UserDto actual = Mapper.Map<UserDto>(input);

        actual.Should().BeEquivalentTo(expected);
    }
}