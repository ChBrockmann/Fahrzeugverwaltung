using Model.User;
using AutoFixture;
using FluentAssertions;

namespace MappingTests;

public class UserMappingTests : TestBase
{
    [Fact]
    public void UserModel_To_UserDto()
    {
        UserModel input = Fixture.Create<UserModel>();
        UserDto expected = new()
        {
            Firstname = input.Firstname,
            Lastname = input.Lastname,
            Id = input.Id,
            Organization = input.Organization
        };

        UserDto actual = Mapper.Map<UserDto>(input);

        actual.Should().BeEquivalentTo(expected);
    }
}