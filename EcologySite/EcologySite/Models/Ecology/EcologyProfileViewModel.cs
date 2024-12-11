namespace EcologySite.Models.Ecology;

public class EcologyProfileViewModel
{
    public string UserName { get; set; }
    public string AvatarUrl { get; set; }
    public List<EcologyForProfileViewModel> Posts { get; set; } 
    public List<CommentForProfileViewModel> Comments { get; set; }
}