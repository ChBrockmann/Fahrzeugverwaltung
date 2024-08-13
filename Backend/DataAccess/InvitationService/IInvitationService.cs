using DataAccess.BaseService;
using Model.Invitation;
using Model.User;

namespace DataAccess.InvitationService;

public interface IInvitationService : IBaseService<InvitationModel, InvitationId>
{
    Task<InvitationModel?> GetByToken(string token);

    Task<bool> SetAcceptedByUser(InvitationId id, UserModel user);
}