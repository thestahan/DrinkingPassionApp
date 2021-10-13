using Core.Models;
using System.IO;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IBlobService
    {
        Task<BlobBasicInfo> GetBlobAsync(string containerName, string blobName);
        Task UploadFileStreamBlobAsync(string containerName, Stream stream, string fileName);
        Task DeleteBlobAsync(string containerName, string blobName);
    }
}
