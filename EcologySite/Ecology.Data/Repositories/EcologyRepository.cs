using Ecology.Data.Interface.Repositories;
using Ecology.Data.Models.Ecology;

namespace Ecology.Data.Repositories;

public interface IEcologyRepositoryReal : IEcologyRepository<EcologyData>
{
    bool IsEclogyTextHas(string text);
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
}