using AbcRetail.Services;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AbcRetail.AzureDemo.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobContainerClient _container;

        public BlobStorageService(IOptions<AzureStorageOptions> options)
        {
            var opts = options.Value;
            var client = new BlobServiceClient(opts.ConnectionString);
            _container = client.GetBlobContainerClient(opts.BlobContainer);
            _container.CreateIfNotExists(PublicAccessType.None);
        }

        public async Task<string> UploadFileAsync(Stream content, string fileName, string contentType = null)
        {
            var blobClient = _container.GetBlobClient(fileName);
            var headers = new BlobHttpHeaders();
            if (!string.IsNullOrEmpty(contentType)) headers.ContentType = contentType;
            await blobClient.UploadAsync(content, new BlobUploadOptions { HttpHeaders = headers });
            return blobClient.Uri.ToString();
        }
    }
}
