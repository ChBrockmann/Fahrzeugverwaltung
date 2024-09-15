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

    public async Task SetRolesOfUser(UserId userId, IEnumerable<Role> roles)
    {
        UserModel? user = await Database.Users
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Id == userId);
        if (user is null) throw new ArgumentNullException(nameof(user), "User should not be null");
        foreach (Role role in roles)
        {
            if (user.Roles.Contains(role)) continue;
            Role? dbRole = await Database.Roles.FirstOrDefaultAsync(x => x.Name == role.Name);
            if (dbRole is null) continue;
            user.Roles.Add(dbRole);
        }

        await Database.SaveChangesAsync();
    }

    public async Task<UserModel?> GetUserByEmail(string email)
    {
        return await Database.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}