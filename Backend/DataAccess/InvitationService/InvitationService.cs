using DataAccess.BaseService;
using Microsoft.EntityFrameworkCore;
using Model.Invitation;
using Model.Roles;
using Model.User;

namespace DataAccess.InvitationService;

public class InvitationService : BaseService<InvitationModel, InvitationId>, IInvitationService
{
    public InvitationService(DatabaseContext database) : base(database) { }

    public override async Task<IEnumerable<InvitationModel>> Get()
    {
        return await Database.InvitationModels
            .Include(x => x.AcceptedBy)
            .ThenInclude(x => x!.Organization)
            .Include(x => x.Roles)
            .Include(x => x.CreatedBy)
            .ToListAsync();
    }

    public override async Task<InvitationModel?> Get(InvitationId id)
    {
        return await Database.InvitationModels
            .Include(x => x.AcceptedBy)
            .Include(x => x.Roles)
            .Include(x => x.CreatedBy)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<InvitationModel?> GetByToken(string token)
    {
        return await Database.InvitationModels
            .Include(x => x.AcceptedBy)
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Token == token);
    }

    public async Task<bool> SetAcceptedByUser(InvitationId id, UserModel user)
    {
        var invitation = await Database.InvitationModels
            .FirstOrDefaultAsync(x => x.Id == id);

        if (invitation is null)
            return false;
        
        invitation.AcceptedBy = user;
        invitation.AcceptedAt = DateTime.Now;

        await Database.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Role>> GetRoles(InvitationId id)
    {
        return await Database.InvitationModels
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Id == id)
            .ContinueWith(x => x.Result?.Roles ?? new List<Role>());
    }
}