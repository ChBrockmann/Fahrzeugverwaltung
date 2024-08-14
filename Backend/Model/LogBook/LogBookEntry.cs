using System.ComponentModel.DataAnnotations;
using Model.User;
using StronglyTypedIds;

namespace Model.LogBook;

[StronglyTypedId(Template.Guid)]
public partial struct LogBookEntryId { }

public sealed record LogBookEntry : IDatabaseId<LogBookEntryId>
{
    public LogBookEntryId Id { get; set; } = LogBookEntryId.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public UserModel CreatedBy { get; set; } = new();
    
    public int? EndMileageInKm { get; set; }
}