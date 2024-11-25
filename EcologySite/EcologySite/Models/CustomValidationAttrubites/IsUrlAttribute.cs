using System.ComponentModel.DataAnnotations;
using System.Resources;

namespace EcologySite.Models.CustomValidationAttrubites
{
    public class IsUrlAttribute : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return string.IsNullOrEmpty(ErrorMessage)
                ? "Not a valid URL."
                : ErrorMessage;
        }
        
        /*public override string FormatErrorMessage(string name)
        {
            if (!string.IsNullOrEmpty(ErrorMessage)) {
                return ErrorMessage;
            }

            if (ErrorMessageResourceType is not null && !string.IsNullOrEmpty(ErrorMessageResourceName))
            {
                var resourceManager = new ResourceManager(ErrorMessageResourceType.FullName,
                    ErrorMessageResourceType.Assembly);
                return resourceManager.GetString(ErrorMessageResourceName);
            }

            return "Не правильный урл";
        }*/

        public override bool IsValid(object? value)
        {
            var url = value as string;
            if (url == null)
            {
                return false;
            }

            if (!url.ToLower().StartsWith("http"))
            {
                return false;
            }

            return true;
        }
    }
}
