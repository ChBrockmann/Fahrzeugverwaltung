using DataAccess.BaseService;
using Model.Invitation;

namespace DataAccess.InvitationService;

public interface IInvitationService : IBaseService<InvitationModel, InivitationId>
{
    Task<InvitationModel?> GetByToken(string token);
}