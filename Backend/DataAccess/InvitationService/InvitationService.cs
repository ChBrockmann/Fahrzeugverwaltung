using DataAccess.BaseService;
using Microsoft.EntityFrameworkCore;
using Model.Invitation;

namespace DataAccess.InvitationService;

public class InvitationService : BaseService<InvitationModel, InivitationId>, IInvitationService
{
    public InvitationService(DatabaseContext database) : base(database) { }
    
    public async Task<InvitationModel?> GetByToken(string token)
    {
        return await Database.InvitationModels
            .Include(x => x.AcceptedBy)
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Token == token);
    }
}