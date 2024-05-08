using Model.User;
using DataAccess.BaseService;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.UserService;

public class UserService : BaseService<UserModel, Guid>, IUserService
{
    public UserService(DatabaseContext databaseContext) : base(databaseContext)
    {
        
    }

    public override async Task<UserModel?> Get(Guid id)
    {
        return await Database.UserModels.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }
}