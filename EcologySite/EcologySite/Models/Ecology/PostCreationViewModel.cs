using EcologySite.Models.CustomValidationAttrubites;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcologySite.Models.Ecology;

public class PostCreationViewModel
{
    [IsUrl(ErrorMessage = "This URL is invalid")]
    public string Url { get; set; }
        
    [EcologyText]
    [IsCorrectLength(15)]
    public string Text { get; set;}
    
    public int PostId { get; set; }
    public List<SelectListItem>? Posts { get; set; }
}