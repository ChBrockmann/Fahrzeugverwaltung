using DataAccess.InvitationService;
using Model;
using Model.Invitation;
using Model.Invitation.Responses;

namespace Fahrzeugverwaltung.Endpoints.InvitationEndpoint;

public class GetAllInvitationsEndpoint : Endpoint<EmptyRequest, GetAllInvitationsResponse>
{
    private readonly IInvitationService _invitationService;
    private readonly IMapper _mapper;

    public GetAllInvitationsEndpoint(IMapper mapper, IInvitationService invitationService)
    {
        _mapper = mapper;
        _invitationService = invitationService;
    }

    public override void Configure()
    {
        Get("invitation");
        Roles(SecurityConfiguration.AdminRoleName);
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var invitations = await _invitationService.Get();
        await SendOkAsync(new GetAllInvitationsResponse()
        {
            Invitations = _mapper.Map<List<InvitationModelDto>>(invitations)
        }, ct);
    }
}