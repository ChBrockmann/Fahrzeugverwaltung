using DataAccess.UserService;
using Model.User;
using Model.User.Responses;

namespace Fahrzeugverwaltung.Endpoints.UserEndpoints;

public class GetAllUserEndpoint : Endpoint<EmptyRequest, AllUserResponse>
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    
    public GetAllUserEndpoint(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Get("user");
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var allUsers = await _userService.Get();

        var response = new AllUserResponse()
        {
            AllUsers = _mapper.Map<List<UserDto>>(allUsers)
        };

        await SendOkAsync(response, ct);
    }
}