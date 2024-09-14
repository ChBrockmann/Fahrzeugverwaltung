using DataAccess.BaseService;
using Model.Invitation;
using Model.Roles;
using Model.User;

namespace DataAccess.InvitationService;

public interface IInvitationService : IBaseService<InvitationModel, InvitationId>
{
    Task<InvitationModel?> GetByToken(string token);

    Task<bool> SetAcceptedByUser(InvitationId id, UserModel user);

    Task<IEnumerable<Role>> GetRoles(InvitationId id);
}