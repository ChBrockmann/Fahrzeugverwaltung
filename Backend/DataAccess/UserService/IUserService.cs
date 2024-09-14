using DataAccess.BaseService;
using Model.Roles;
using Model.User;

namespace DataAccess.UserService;

public interface IUserService : IBaseService<UserModel, UserId>
{
    public Task<List<Role>> GetRolesOfUser(UserId userId);
    
    public Task<UserModel?> GetUserByEmail(string email);
}