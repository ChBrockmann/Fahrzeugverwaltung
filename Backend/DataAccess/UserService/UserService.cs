using DataAccess.BaseService;
using Microsoft.EntityFrameworkCore;
using Model.Roles;
using Model.User;

namespace DataAccess.UserService;

public class UserService : BaseService<UserModel, UserId>, IUserService
{
    public UserService(DatabaseContext databaseContext) : base(databaseContext)
    {
        
    }

    public override async Task<UserModel?> Get(UserId id)
    {
        return await Database.Users
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Role>> GetRolesOfUser(UserId userId)
    {
        return await Database.Users
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Id == userId)
            .ContinueWith(x => x.Result?.Roles ?? new List<Role>());
    }

    public async Task<UserModel?> GetUserByEmail(string email)
    {
        return await Database.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}