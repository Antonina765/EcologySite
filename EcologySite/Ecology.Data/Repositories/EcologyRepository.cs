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
        // First в целом лучше не использовать потому что он выбрасывает исключение, что очень затратно
        // лучше написать FirstOrDefault(...) и потом сделать типо if(creator == null) {и тут делаешь логику которая нужна, например пишешь просто выход из метода}
        
        var creator = _webDbContext.Users.First(x => x.Id == currentUserId);
        
        // если ты хочешь получить все комментарии к посту, то тебе нужно просто найти их через Where(...)
        // тут у тебя Comments это не коллекция, а просто CommentData, потому что ты пишешь First() (этот метод выбирает первую штуку совпадающую с условием)
        
        var comments = _webDbContext.Comments.First(x => x.Id == postId);

        ecology.User = creator;
        ecology.Comments = comments;

        Add(ecology);
    }
}