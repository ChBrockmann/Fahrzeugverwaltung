using Microsoft.EntityFrameworkCore;
using Model.Roles;

namespace DataAccess.RoleService;

public class RoleService : IRoleService
{
    private readonly DatabaseContext _database;

    public RoleService(DatabaseContext database)
    {
        _database = database;
    }

    public async Task<Role> Create(Role create)
    {
        await _database.Roles.AddAsync(create);
        return create;
    }

    public async Task<IEnumerable<Role>> Get()
    {
        return await _database.Roles
            .ToListAsync();
    }

    public async Task<Role?> Get(string id)
    {
        return await _database.Roles
            .FirstOrDefaultAsync(x => x.Name == id);
    }

    public async Task<Role?> Update(Role update)
    {
        Role? role = await Get(update.Name);
        if (role == null) return null;

        role.Users = update.Users;
        await _database.SaveChangesAsync();
        return role;
    }

    public async Task<bool> Delete(string id)
    {
        Role? role = await Get(id);
        if (role == null) return false;

        _database.Roles.Remove(role);
        await _database.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Exists(string id)
    {
        return await _database.Roles
            .AnyAsync(x => x.Name == id);
    }
}