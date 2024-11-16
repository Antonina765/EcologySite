using System.ComponentModel.DataAnnotations;
using Ecology.Data.Repositories;
using EcologySite.Services;

namespace EcologySite.Models.CustomValidationAttrubites;

public class EcologyTextAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(
        object? value, 
        ValidationContext validationContext)
    {
        var text = value as string;
        if (text == null)
        {
            return new ValidationResult("Not a string");
        }

        var repository = validationContext.GetRequiredService<IEcologyRepositoryReal>();
        if (CalcCountWorldRepeat.IsEclogyTextHas(text)>=4)
        {
            return new ValidationResult("Not uniq text");
        }

        return ValidationResult.Success;
    }
}