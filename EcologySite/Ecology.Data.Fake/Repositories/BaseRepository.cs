using Ecology.Data.Interface.Models;
using Ecology.Data.Interface.Repositories;


namespace Everything.Data.Fake.Repositories;

public class BaseRepository<T> : IBaseRepository<T>
        where T : IBaseModel
{
    protected List<T> _entyties = new List<T>();

    public void Add(T data)
    {
        data.Id = _entyties.Any()
            ? _entyties.Max(x => x.Id) + 1
            : 1;

        _entyties.Add(data);
    }

    public void Delete(T data)
    {
        _entyties.Remove(data);
    }

    public void Delete(int id)
    {
        var data = Get(id);
        Delete(data);
    }

    public IEnumerable<T> GetAll()
    {
        return _entyties;
    }

    public T? Get(int id)
    {
        return _entyties.FirstOrDefault(x => x.Id == id);
    }

    public bool Any()
    {
        return _entyties.Any();
    }
}