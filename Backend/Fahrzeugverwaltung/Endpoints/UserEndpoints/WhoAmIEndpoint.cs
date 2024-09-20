using DataAccess.UserService;
using Model.User;
using Model.User.Responses;

namespace Fahrzeugverwaltung.Endpoints.UserEndpoints;

public class WhoAmIEndpoint : BaseEndpoint<EmptyRequest, WhoAmIResponse>
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

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
        UserId userId = UserFromContext.Id;
        UserModel? requestingUser = await _userService.Get(userId);

        if (requestingUser is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        List<string> roles = (await _userService.GetRolesOfUser(userId))
            .Select(x => x.Name)
            .ToList();
        
        await SendOkAsync(new WhoAmIResponse
        {
            User = _mapper.Map<UserDto>(requestingUser),
            Roles = roles
        }, ct);
    }
}