using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace REBrokerApp.Infrastructure.Extensions
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxSize;
        public MaxFileSizeAttribute(int maxSize)
        {
            _maxSize = maxSize;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var files = value as List<IFormFile>;
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file.Length > _maxSize)
                    {
                        return new ValidationResult($"File '{file.FileName}' exceeds maximum size of {_maxSize / 1024 / 1024}MB.");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }

    // Attributes/AllowedExtensionsAttribute.cs
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var files = value as List<IFormFile>;
            if (files != null)
            {
                foreach (var file in files)
                {
                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (Array.IndexOf(_extensions, extension) < 0)
                    {
                        return new ValidationResult($"File '{file.FileName}' has invalid extension.");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
