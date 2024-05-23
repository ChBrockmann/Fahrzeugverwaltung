using Microsoft.EntityFrameworkCore;
using Model;

namespace DataAccess.BaseService;

public class BaseService<TObject, TId> : IBaseService<TObject, TId> where TObject : class
{
    private readonly DbSet<TObject> _dbSet;
    protected readonly DatabaseContext Database;

    public BaseService(DatabaseContext database)
    {
        Database = database;
        _dbSet = database.Set<TObject>();
    }

    public virtual async Task<TObject> Create(TObject objectToCreate)
    {
        if (objectToCreate == null) throw new ArgumentNullException();
        IDatabaseId<TId> casted = objectToCreate as IDatabaseId<TId> ?? throw new InvalidCastException();

        TId id = CheckForNonDefaultValue(casted);
        casted.Id = id;

        _dbSet.Add(objectToCreate);
        await Database.SaveChangesAsync();

        return (await Get(casted.Id))!;
    }

    public virtual async Task<TObject?> Get(TId id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<TObject>> Get()
    {
        return await Task.FromResult(_dbSet);
    }

    public virtual async Task<TObject?> Update(TObject updatedEntry)
    {
        IDatabaseId<TId> casted = updatedEntry as IDatabaseId<TId> ?? throw new InvalidCastException();
        TObject? dbEntry = await _dbSet.FindAsync(casted.Id);

        if (dbEntry is null)
            return null;

        Database.Entry(dbEntry).CurrentValues.SetValues(updatedEntry);

        await Database.SaveChangesAsync();

        return await Get(casted.Id);
    }

    public virtual async Task<bool> Delete(TId id)
    {
        TObject? entry = await _dbSet.FindAsync(id);
        if (entry is null) return false;

        _dbSet.Remove(entry);
        await Database.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Exists(TId id)
    {
        return await _dbSet.FindAsync(id) is not null;
    }

    public TId CheckForNonDefaultValue(IDatabaseId<TId> input)
    {
        if (input.Id!.ToString() == Guid.Empty.ToString()) return (TId) Activator.CreateInstance(typeof(TId), Guid.NewGuid())!;

        if (input.Id.ToString() == string.Empty) return (TId) Activator.CreateInstance(typeof(TId), Guid.NewGuid().ToString())!;

        return input.Id;
    }
}