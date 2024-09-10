using DataAccess.BaseService;
using Model.Organization;

namespace DataAccess.OrganizationService;

public class OrganizationService : BaseService<Organization, OrganizationId>, IOrganizationService
{
    public OrganizationService(DatabaseContext database) : base(database) { }
}