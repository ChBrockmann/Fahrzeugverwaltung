using DataAccess.BaseService;
using Model.Organization;

namespace DataAccess.OrganizationService;

public class OrganizationService : BaseService<OrganizationModel, OrganizationId>, IOrganizationService
{
    public OrganizationService(DatabaseContext database) : base(database) { }
}