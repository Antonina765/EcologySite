using System.ComponentModel.DataAnnotations;

namespace EcologySite.Models.CustomValidationAttrubites;

public class IsCorrectLengthAttribute : ValidationAttribute
{
    private int _max;

    public IsCorrectLengthAttribute()
    {
        _max = 50;
    }

    public IsCorrectLengthAttribute(int max)
    {
        _max = max;
    }

    public override string FormatErrorMessage(string name)
    {
        return string.IsNullOrEmpty(ErrorMessage)
            ? $"Text size must not exceed {_max}"
            : ErrorMessage;
    }

    public override bool IsValid(object? value)
    {
        if (value is not int)
        {
            return false;
        }

        var length = (int)value;

        if (length > _max)
        {
            return false;
        }

        return true;
    }
}