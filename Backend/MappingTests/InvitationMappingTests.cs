using AutoFixture;
using FluentAssertions;
using Model.Invitation;
using Model.User;

namespace MappingTests;

public class InvitationMappingTests : TestBase
{
    [Fact]
    public void InvitationModel_To_InvitationModelDto()
    {
        InvitationModel input = Fixture.Create<InvitationModel>();
        InvitationModelDto expected = new()
        {
            Id = input.Id,
            Token = input.Token,
            ExpiresAt = input.ExpiresAt,
            CreatedBy = input.CreatedBy is not null ? Mapper.Map<UserDto>(input.CreatedBy) : null,
            CreatedAt = input.CreatedAt,
            AcceptedBy = input.AcceptedBy is not null ? Mapper.Map<UserDto>(input.AcceptedBy) : null,
            AcceptedAt = input.AcceptedAt,
            Roles = input.Roles.Select(x => x.Name ?? string.Empty).ToList()
        };

        InvitationModelDto actual = Mapper.Map<InvitationModelDto>(input);
        actual.Should().BeEquivalentTo(expected);
    }
}