using DataAccess.BaseService;
using Model.Invitation;
using Model.User;

namespace DataAccess.InvitationService;

public interface IInvitationService : IBaseService<InvitationModel, InivitationId>
{
    Task<InvitationModel?> GetByToken(string token);

    Task<bool> SetAcceptedByUser(InivitationId id, UserModel user);
}