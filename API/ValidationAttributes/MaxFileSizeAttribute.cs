using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace API.ValidationAttributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;

        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult($"Maksymalny dozwolony rozmiar pliku to {_maxFileSize / 1024 / 1024} MB");
                }
            }

            return ValidationResult.Success;
        }
    }
}
