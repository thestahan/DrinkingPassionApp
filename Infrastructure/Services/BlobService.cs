using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public Task DeleteBlobAsync(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            throw new NotImplementedException();
        }

        public async Task<BlobBasicInfo> GetBlobAsync(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = containerClient.GetBlobClient(blobName);

            var blobDownloadInfo = await blobClient.DownloadAsync();

            return new BlobBasicInfo(blobDownloadInfo.Value.Content, blobDownloadInfo.Value.ContentType);
        }

        public async Task UploadFileStreamBlobAsync(string containerName, Stream stream, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = fileName.GetContentType() });
        }
    }
}
