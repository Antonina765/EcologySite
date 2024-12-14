namespace EcologySite.Models.Ecology;

public class CommentForProfileViewModel
{
    public int CommentId { get; set; }
    public string CommentText { get; set; }
    public EcologyViewModel PostText { get; set; }
}