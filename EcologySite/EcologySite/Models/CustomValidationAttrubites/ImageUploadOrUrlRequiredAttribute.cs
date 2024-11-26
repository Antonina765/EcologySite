using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace EcologySite.Models.CustomValidationAttrubites;

public class ImageUploadOrUrlRequiredAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var instance = validationContext.ObjectInstance;
        var instanceType = instance.GetType();
        var urlProperty = instanceType.GetProperty("Url");
        var imageFileProperty = instanceType.GetProperty("ImageFile");

        if (urlProperty == null || imageFileProperty == null)
        {
            throw new ArgumentException("Properties 'Url' and 'ImageFile' must be defined on the object.");
        }

        var urlValue = urlProperty.GetValue(instance) as string;
        var imageFileValue = imageFileProperty.GetValue(instance) as IFormFile;

        if (string.IsNullOrEmpty(urlValue) && imageFileValue == null)
        {
            return new ValidationResult("Please provide either an image URL or upload an image.", new[] { "Url", "ImageFile" });
        }

        if (!string.IsNullOrEmpty(urlValue) && imageFileValue != null)
        {
            return new ValidationResult("Please provide either an image URL or upload an image, not both.", new[] { "Url", "ImageFile" });
        }

        return ValidationResult.Success;
    }
}