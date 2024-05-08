namespace DataAccess.BaseService;

public interface IBaseService<TObject, in TId> where TObject : class
{
    public Task<TObject> Create(TObject create);

    public Task<IEnumerable<TObject>> Get();
    public Task<TObject?> Get(TId id);

    public Task<TObject?> Update(TObject update);

    public Task<bool> Delete(TId id);
}