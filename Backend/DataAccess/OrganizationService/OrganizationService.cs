using DataAccess.BaseService;
using Model.Organization;

namespace DataAccess.OrganizationService;

public class OrganizationService : BaseService<OrganizationModel, OrganizationId>, IOrganizationService
{
    public OrganizationService(DatabaseContext database) : base(database) { }

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
}