namespace EcologySite.Models.Ecology;

public class EcologyViewModel
{
    public int PostId { get; set; }
    public string ImageSrc { get; set; }
    public string Texts { get; set; }
    
    public string UserId { get; set; }
    public string UserAvatar { get; set; } 
    public string UserName { get; set; }
    //public EcologyUserViewModel User { get; set; }
    public ICollection<CommentViewModel> Comments { get; set; }
    
    public bool CanDelete { get; set; }
}