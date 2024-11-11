using Ecology.Data.Interface.Repositories;
using Ecology.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecology.Data.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T>
    where T : BaseModel
{
    protected WebDbContext _webDbContext;
    protected DbSet<T> _dbSet;

    public BaseRepository(WebDbContext webDbContext)
    {
        _webDbContext = webDbContext;
        _dbSet = webDbContext.Set<T>();
    }

    public void Add(T data)
    {
        _webDbContext.Add(data);
        _webDbContext.SaveChanges();
    }

    public bool Any()
    {
        return _dbSet.Any();
    }

    public void Delete(T data)
    {
        _dbSet.Remove(data);
        _webDbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        var data = Get(id);
        Delete(data);
    }

    public T? Get(int id)
    {
        return _dbSet.FirstOrDefault(x => x.Id == id);
    }

    public IEnumerable<T> GetAll()
    {
        return _dbSet.ToList();
    }
}