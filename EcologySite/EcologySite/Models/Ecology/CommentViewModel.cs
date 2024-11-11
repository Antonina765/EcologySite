namespace EcologySite.Models.Ecology;

public class CommentViewModel
{
    public int CommentId { get; set; }
    public string CommentText { get; set; }
    public int PostId { get; set; }
    public EcologyViewModel Post { get; set; }
    public string UserId { get; set; }
    //public EcologyUserViewModel User { get; set; }
}