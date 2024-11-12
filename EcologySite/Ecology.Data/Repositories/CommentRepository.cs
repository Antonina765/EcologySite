using Ecology.Data.Interface.Repositories;
using Ecology.Data.Models;
using Ecology.Data.Models.Ecology;

namespace Ecology.Data.Repositories;

public interface ICommentRepositoryReal : ICommentRepository<CommentData>
{
    public object Comments { get; set; }
    public void Add(CommentData comment);
}

public class CommentRepository : BaseRepository<CommentData>, ICommentRepositoryReal
{
    public CommentRepository(WebDbContext webDbContext) : base(webDbContext)
    {
    }

    public object Comments { get; set; }
}