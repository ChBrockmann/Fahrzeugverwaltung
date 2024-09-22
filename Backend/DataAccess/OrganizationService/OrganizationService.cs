using DataAccess.BaseService;
using Microsoft.EntityFrameworkCore;
using Model.Organization;
using Model.User;

namespace DataAccess.OrganizationService;

public class OrganizationService : BaseService<OrganizationModel, OrganizationId>, IOrganizationService
{
    public OrganizationService(DatabaseContext database) : base(database) { }

    public override async Task<IEnumerable<OrganizationModel>> Get()
    {
        return await Database.Organizations
            .Include(x => x.Admins)
            .ToListAsync();
    }

    public async Task<OrganizationModel> GetOrCreate(string name)
    {
        OrganizationModel? organization = Database.Organizations.FirstOrDefault(o => o.Name == name);
        if (organization is null)
        {
            organization = new OrganizationModel
            {
                Id = OrganizationId.New(),
                Name = name
            };
            await Create(organization);
        }

        return organization;
    }

    public async Task SetOrganizationAdmin(OrganizationId organizationId, UserModel user)
    {
        var organization = await Database.Organizations
            .Include(x => x.Admins)
            .FirstOrDefaultAsync(x => x.Id == organizationId);

        if (organization is null)
        {
            throw new Exception("Organization not found");
        }
        
        organization.Admins.Add(user);

        await Database.SaveChangesAsync();
    }

    public async Task RemoveOrganizationAdmin(OrganizationId organizationId, UserId userId)
    {
        var organization = await Database.Organizations
            .Include(x => x.Admins)
            .FirstOrDefaultAsync(x => x.Id == organizationId);

        if (organization is null)
        {
            throw new Exception("Organization not found");
        }

        var user = organization.Admins.FirstOrDefault(x => x.Id == userId);
        if (user is null)
        {
            return;
        }

        organization.Admins.Remove(user);

        await Database.SaveChangesAsync();
    }
}