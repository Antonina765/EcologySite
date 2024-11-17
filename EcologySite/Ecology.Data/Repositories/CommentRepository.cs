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

        // Проверка, есть ли у пользователя посты
        var usersPosts = _dbSet
            .Where(post => post.User != null 
                           && post.User.Id == userId);
        
        // Проверка, есть ли комментарии у постов пользователя
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

            return authorsPosts.Union(notAuthorsPosts).ToList();
        }
}