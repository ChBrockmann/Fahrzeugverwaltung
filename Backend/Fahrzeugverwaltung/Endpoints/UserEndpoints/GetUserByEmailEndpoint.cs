using DataAccess.UserService;
using Model.User;
using Model.User.Request;
using Model.User.Responses;

namespace Fahrzeugverwaltung.Endpoints.UserEndpoints;

public class GetUserByEmailEndpoint : Endpoint<GetUserByEmailRequest, GetUserByEmailResponse>
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    
    public GetUserByEmailEndpoint(IMapper mapper, IUserService userService)
    {
        _mapper = mapper;
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/user/{Email}");
    }

    public override async Task HandleAsync(GetUserByEmailRequest req, CancellationToken ct)
    {
        var allUsers = await _userService.Get();
        UserModel? user = allUsers.FirstOrDefault(x => x.Email == req.Email);

        if (user is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendOkAsync(new()
        {
            User = _mapper.Map<UserDto>(user)
        }, ct);
    }
}