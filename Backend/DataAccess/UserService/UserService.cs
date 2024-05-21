using DataAccess.BaseService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.User;

namespace DataAccess.UserService;

public class UserService : BaseService<UserModel, Guid>, IUserService
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public UserService(DatabaseContext databaseContext, RoleManager<IdentityRole<Guid>> roleManager) : base(databaseContext)
    {
        _roleManager = roleManager;
    }

    public override async Task<UserModel?> Get(Guid id)
    {
        return await Database.UserModels.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public async Task<List<string>> GetRolesOfUser(Guid userId)
    {
        var roleIds = await Database.UserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).ToListAsync();

        return await Database.Roles.Where(x => roleIds.Contains(x.Id)).Select(x => x.Name ?? "").ToListAsync();
    }
}