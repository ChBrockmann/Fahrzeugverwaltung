using DataAccess.BaseService;
using Model.Organization;
using Model.User;

namespace DataAccess.OrganizationService;

public interface IOrganizationService : IBaseService<OrganizationModel, OrganizationId>
{
    public Task<OrganizationModel> GetOrCreate(string name);

    public Task SetOrganizationAdmin(OrganizationId organizationId, UserModel user);
    public Task RemoveOrganizationAdmin(OrganizationId organizationId, UserId userId);
}