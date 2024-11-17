using Ecology.Data.Interface.Models;
using Ecology.Data.Models.Ecology;

namespace Ecology.Data.Models;

public class CommentData : BaseModel, ICommentData
{
    //private ICommentData _commentDataImplementation;
    public int PostId { get; set; } 
    public string CommentText { get; set; }
    public int UserId { get; set; }
    public EcologyData Ecology { get; set; }
    public UserData? User { get; set; }
}