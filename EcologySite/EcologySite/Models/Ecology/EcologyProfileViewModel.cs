namespace EcologySite.Models.Ecology;

public class EcologyProfileViewModel
{
    public string UserName { get; set; }
    public List<EcologyViewModel> Posts { get; set; } 
    public List<CommentViewModel> Comments { get; set; }
}