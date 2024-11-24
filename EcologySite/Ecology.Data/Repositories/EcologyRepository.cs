using Ecology.Data.Interface.Repositories;
using Ecology.Data.Models.Ecology;
using Everything.Data.DataLayerModels;
using Microsoft.EntityFrameworkCore;

namespace Ecology.Data.Repositories;

public interface IEcologyRepositoryReal : IEcologyRepository<EcologyData>
{
    void Create(EcologyData ecology, int currentUserId, int postId);
    IEnumerable<EcologyData>GetAllWithUsersAndComments();
    
    void PostsForMainPage(EcologyData ecology);
    
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
    
    public void PostsForMainPage(EcologyData ecology)
    {
        _dbSet.Update(ecology); 
        _webDbContext.SaveChanges();
    }
    
    public IEnumerable<EcologyData> GetAllWithUsersAndComments()
    {
        return _dbSet
            .Include(x => x.User)
            .Include(x => x.Comments)
            .ToList();
    }
   
    public void Create(EcologyData ecology, int currentUserId, int postId)
    {
        var creator = _webDbContext.Users.FirstOrDefault(x => x.Id == currentUserId);
        
        var comments = _webDbContext.Comments.Where(x => x.Id == postId);

        ecology.User = creator;
        ecology.Comments = comments;

        Add(ecology);
    }
}