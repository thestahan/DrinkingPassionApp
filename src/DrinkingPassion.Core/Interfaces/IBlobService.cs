using System.IO;
using System.Threading.Tasks;

namespace DrinkingPassion.Api.Core.Interfaces
{
    public interface IBlobService
    {
        Task<Models.BlobBasicInfo> GetBlobAsync(string containerName, string blobName);

        Task UploadFileStreamBlobAsync(string containerName, Stream stream, string fileName);

        Task DeleteBlobAsync(string containerName, string blobName);
    }
}
