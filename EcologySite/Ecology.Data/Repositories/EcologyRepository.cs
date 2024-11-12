using Ecology.Data.Interface.Models;
using Ecology.Data.Interface.Repositories;
using Ecology.Data.Models;
using Ecology.Data.Models.Ecology;
using Everything.Data.Fake.Repositories;

namespace Ecology.Data.Repositories;
public interface IEcologyRepositoryReal : IEcologyRepository<EcologyData>
{
    public void Add(EcologyData ecology);
}

public class EcologyRepository : BaseRepository<EcologyData>, IEcologyRepositoryReal
{
    public EcologyRepository(WebDbContext webDbContext) : base(webDbContext)
    {
    }

    public void UpdatePost(int id, string url, string text)
    {
        var ecology = _dbSet.First(e => e.Id == id); 

        ecology.ImageSrc = url;
        ecology.Text = text; 
                
        _webDbContext.SaveChanges();
    }

    public void Add(EcologyData ecology)
    {
        throw new NotImplementedException();
    }
}