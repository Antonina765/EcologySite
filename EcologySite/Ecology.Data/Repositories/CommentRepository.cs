using Ecology.Data.Interface.Repositories;
using Ecology.Data.Models;
using Everything.Data.DataLayerModels;

namespace Ecology.Data.Repositories;

public interface ICommentRepositoryReal : ICommentRepository<CommentData>
{
    IEnumerable<CommentData> GetCommentsByPostId(int postId);
    CommentsAndPostsByUser GetCommentAuthors(int userId);
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

    // Поменяй название метода потому что оно ВООБЩЕ не подходит
    
    // в том как придумал я не вижу смысла оставлять IEnumerable
    public CommentsAndPostsByUser GetCommentAuthors(int userId)
    {
        // мы получаем юзера, потому в нем лежат и комменты и посты которые он оставил, и уже от него мы пойдем дальше
        var user = (_dbSet.FirstOrDefault(us => us.UserId == userId))?.User; // собственно получаем юзера

        if (user is null) // нужно узнать все ли ок и проверить на null
            return null;  
        // возращаем null чтобы код который дальше не выполняли
        // todo нужно в контроллере это тоже проверять и в случае чего показывать что была ошибка в ui 
        // что то типо "произошла ошибка при получении комментариев и постов"

        var comments = user.Comments;       // получаем все комментарии пользователя
        var posts = user.Ecologies;          // получаем все посты пользователя

        if (comments is null || posts is null)                      // снова проверяем на null, по той же причине что и выше
            return null;

        return new CommentsAndPostsByUser(userId, comments.ToList(), posts.ToList());     
        // посмотри этот класс чтобы понять, там просто контруктор и свойства
    }
}