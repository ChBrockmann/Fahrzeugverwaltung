using DataAccess.BaseService;
using Model.Organization;

namespace DataAccess.OrganizationService;

public interface IOrganizationService : IBaseService<OrganizationModel, OrganizationId>
{
    public Task<OrganizationModel> GetOrCreate(string name);
}