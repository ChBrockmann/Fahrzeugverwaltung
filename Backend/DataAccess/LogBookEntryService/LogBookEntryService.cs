using DataAccess.BaseService;
using Model.LogBook;

namespace DataAccess.LogBookEntryService;

public class LogBookEntryService : BaseService<LogBookEntry, LogBookEntryId>, ILogBookEntryService
{
    public LogBookEntryService(DatabaseContext database) : base(database) { }
}