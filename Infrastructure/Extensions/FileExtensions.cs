using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public static class FileExtensions
    {
        private static readonly FileExtensionContentTypeProvider Provider = new();

        public static string GetContentType(this string fileName)
        {
            if (!Provider.TryGetContentType(fileName, out string contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }

        public static async Task<string> ReadFormFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return await Task.FromResult((string)null);
            }

            using var reader = new StreamReader(file.OpenReadStream());

            return await reader.ReadToEndAsync();
        }
    }
}
