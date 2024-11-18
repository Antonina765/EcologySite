namespace EcologySite.Models.Ecology;

public class EcologyProfileViewModel
{
    public string UserName { get; set; }
    public List<EcologyForProfileViewModel> Posts { get; set; } 
    public List<CommentViewModel> Comments { get; set; }
}