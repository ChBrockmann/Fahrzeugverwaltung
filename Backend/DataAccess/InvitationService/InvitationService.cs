using DataAccess.BaseService;
using Model.Invitation;

namespace DataAccess.InvitationService;

public class InvitationService : BaseService<InvitationModel, InivitationId>, IInvitationService
{
    public InvitationService(DatabaseContext database) : base(database) { }
}