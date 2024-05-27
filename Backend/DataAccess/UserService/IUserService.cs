using DataAccess.BaseService;
using Model.User;

namespace DataAccess.UserService;

public interface IUserService : IBaseService<UserModel, Guid>
{
    public Task<List<string>> GetRolesOfUser(Guid userId);
    
    public Task<UserModel?> GetUserByEmail(string email);
}