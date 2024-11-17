using Ecology.Data.Interface.Repositories;
using Ecology.Data.Models.Ecology;
using Microsoft.EntityFrameworkCore;

namespace Ecology.Data.Repositories;

public interface IEcologyRepositoryReal : IEcologyRepository<EcologyData>
{
    bool IsEclogyTextHas(string text);
    void Create(EcologyData ecology, int currentUserId, int postId);
    IEnumerable<EcologyData>GetAllWithUsersAndComments();
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
    
    public bool IsEclogyTextHas(string text)
    {
        var words = text.Split(new[] { ' ', '.', ',', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        var wordCounts = new Dictionary<string, int>();

        foreach (var word in words)
        {
            var lowerWord = word.ToLowerInvariant();
            if (wordCounts.ContainsKey(lowerWord))
            {
                wordCounts[lowerWord]++;
            }
            else
            {
                wordCounts[lowerWord] = 1;
            }
        }

        return !wordCounts.Any(kv => kv.Value >= 4);
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
        var creator = _webDbContext.Users.First(x => x.Id == currentUserId);
        var comments = _webDbContext.Comments.First(x => x.Id == postId);

        ecology.User = creator;
        ecology.Comments = comments;

        Add(ecology);
    }
}