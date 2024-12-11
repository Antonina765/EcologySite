using Ecology.Data.Models;
using Ecology.Data.Models.Ecology;

namespace Ecology.Data.DataLayerModels
{
    public class CommentsAndPostsByUser
    {
        public CommentsAndPostsByUser(int userId, List<CommentData> comments, List<EcologyData> posts)
        {
            UserId = userId;
            Comments = comments;
            Posts = posts;
        }

        public int UserId { get; set; }
        public List<CommentData> Comments { get; set; }
        public List<EcologyData> Posts { get; set; }
    }
}
