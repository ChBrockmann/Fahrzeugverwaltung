using DataAccess.BaseService;
using Model.Roles;
using Model.User;

namespace DataAccess.UserService;

public interface IUserService : IBaseService<UserModel, UserId>
{
    public Task<List<Role>> GetRolesOfUser(UserId userId);
    public Task SetRolesOfUser(UserId userId, IEnumerable<Role> roles);
    
    public Task<UserModel?> GetUserByEmail(string email);
    public Task SetAuthIdOfUser(UserId userId, string authId);
}