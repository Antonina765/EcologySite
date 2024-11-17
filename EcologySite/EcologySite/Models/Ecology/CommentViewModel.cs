namespace EcologySite.Models.Ecology;

public class CommentViewModel
{
    public int CommentId { get; set; }
    public string CommentText { get; set; }
    public int PostId { get; set; }
    public string UserAvatar { get; set; } 
    public string UserName { get; set; }
    public EcologyViewModel PostText { get; set; }
    public string UserId { get; set; }
    //public EcologyUserViewModel User { get; set; }
}