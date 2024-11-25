using System.ComponentModel.DataAnnotations;
using EcologySite.Models.CustomValidationAttrubites;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcologySite.Models.Ecology;

public class PostCreationViewModel
{
    [IsUrl(ErrorMessage = "This URL is invalid")]
    /*[IsUrl(
        ErrorMessageResourceType = typeof(Girl), 
        ErrorMessageResourceName = nameof(Girl.ValidationMessage_IsNotUrl))]*/

    public string Url { get; set; }

    [Required(ErrorMessage = "Text is required.")]
    [EcologyText, IsCorrectLength(15)] 
    public string Text { get; set;}
    
    public int PostId { get; set; }
    public List<SelectListItem>? Posts { get; set; }

    [MaxFileSize(52428800)]
    [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Please upload a valid image file (jpg, jpeg, png).")]
    public IFormFile ImageFile { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(Url) && ImageFile == null)
        {
            yield return new ValidationResult("Please provide either an image URL or upload an image.",
                new[] { nameof(Url), nameof(ImageFile) });
        }

        if (!string.IsNullOrEmpty(Url) && ImageFile != null)
        {
            yield return new ValidationResult("Please provide either an image URL or upload an image, not both.",
                new[] { nameof(Url), nameof(ImageFile) });
        }
    }
}