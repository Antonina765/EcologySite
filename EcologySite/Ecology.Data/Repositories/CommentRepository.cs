using Ecology.Data.Interface.Repositories;
using Ecology.Data.Models;
using Everything.Data.DataLayerModels;

namespace Ecology.Data.Repositories;

public interface ICommentRepositoryReal : ICommentRepository<CommentData>
{
    IEnumerable<CommentData> GetCommentsByPostId(int postId);
    IEnumerable<CommentAuthor> GetCommentAuthors(int userId);
}

public class CommentRepository : BaseRepository<CommentData>, ICommentRepositoryReal
{
    public CommentRepository(WebDbContext webDbContext) : base(webDbContext)
    {
    }

    public IEnumerable<CommentData> GetCommentsByPostId(int postId)
    {
        return _dbSet.Where(c => c.PostId == postId).ToList();
    }
    
    public IEnumerable<CommentAuthor> GetCommentAuthors(int userId)
    {
        // Проверка, есть ли у пользователя комментарии
        var usersComments = _dbSet
            .Where(comment => comment.User != null 
                              && comment.User.Id == userId);
        var comments = _dbSet
            .Where(x => 
                x.UserId == userId)
            .ToList();

        // Проверка, есть ли у пользователя посты
        var usersPosts = _dbSet
            .Where(post => post.User != null 
                           && post.User.Id == userId);
        var posts = _dbSet
            .Where(x => 
                x.UserId == userId)
            .ToList();
        
        
        /* Проверка, есть ли комментарии у постов пользователя

        // Для того чтобы ты могла применять Linq запросы, нужно чтобы коллекция была IEnumerable (про Where)
        // Ecology у тебя не является коллекцией вообще, поэтому ты не можешь применять Linq запросы (про Any и Select)
        var authorsPosts = usersComments
            .Where(comment => 
                comment
                    .Ecology
                    .Any(post => post.User != null 
                                 && post.User.Id == userId))
            .Select(x => new CommentAuthor
                {
                    Name = x.Title,
                    HasCommentAuther = true
                });

        // Проверка, есть ли комментарии у постов, не созданных пользователем
        var notAuthorsPosts = usersComments
            .Where(comment => 
                !comment
                    .Ecology
                    .Any(post => post.User != null 
                                 && post.User.Id == userId))
            .Select(x => new CommentAuthor
                {
                    Name = x.Title,
                    HasCommentAuther = false
                });

        
        // зачем вообще так мудрить, ты получила в первой части посты, затем в этих постах ищешь его комментарии и все
        // (как я понял ты хочешь получить комментарии пользователя только у постов, которые он и создал)
        // если нет, то все НАМНОГО проще, тебе нужно просто взять комментарии и выбрать из них комментарии с его айди
        // будет типо так: var comments = _dbSet.Where(x => x.UserId == userId).ToList();
            return authorsPosts.Union(notAuthorsPosts).ToList();*/
        return comments + posts;
        }
}