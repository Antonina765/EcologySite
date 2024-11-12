using System.ComponentModel.DataAnnotations;

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
