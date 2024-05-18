using System.Security.Claims;
using DataAccess.UserService;
using Model.User;
using Model.User.Responses;

namespace Fahrzeugverwaltung.Endpoints.UserEndpoints;

public class WhoAmIEndpoint : Endpoint<EmptyRequest, WhoAmIResponse>
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    
    public WhoAmIEndpoint(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Get("user/whoami");
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var claimUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (claimUserId is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        Guid userId = Guid.Parse(claimUserId);
        UserModel? requestingUser = await _userService.Get(userId);
        
        if (requestingUser is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendOkAsync(new()
        {
            User = _mapper.Map<UserDto>(requestingUser)
        }, ct);
    }
}