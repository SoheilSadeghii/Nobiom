using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nobiom.Application.Security
{
    public static class FileValidationExtension
    {
        public static bool ImageValidate(this IFormFile image)
        {
            string[] allowedExtensions =
            {
                ".jpg",
                "jpeg",
                ".webp",
                ".png"
            };

            var fileExtension = Path.GetExtension(image.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                return false;
            }

            return true;
        }
    }
}
