using Azure.Storage.Files.Shares;
using Azure;
using Microsoft.Extensions.Options;
using Azure.Storage.Files.Shares.Models;
using AbcRetail.Models;

namespace AbcRetail.Services
{
    public class FileShareService : IFileShareService
    {
        private readonly ShareClient _shareClient;

        /* public FileShareService(IOptions<AzureStorageOptions> options)
         {
             var opts = options.Value;
             var serviceClient = new ShareServiceClient(opts.ConnectionString);
             _shareClient = serviceClient.GetShareClient(opts.FileShareName);
             EnsureShareExistsAsync().GetAwaiter().GetResult();
         }*/

        //
        public FileShareService(IOptions<AzureStorageOptions> options)
        {
            _shareClient = new ShareClient(options.Value.ConnectionString, "logs"); // "logs" is the share name
            _shareClient.CreateIfNotExists();
        }

        public async Task<List<string>> GetLogFilesAsync()
        {
            var logFiles = new List<string>();
            await foreach (ShareFileItem item in _shareClient.GetRootDirectoryClient().GetFilesAndDirectoriesAsync())
            {
                logFiles.Add(item.Name);
            }
            return logFiles;
        }
        //

        public async Task EnsureShareExistsAsync()
        {
            await _shareClient.CreateIfNotExistsAsync();
        }

        public async Task UploadLogFileAsync(string pathWithinShare, Stream content)
        {
            // 
           // Split the path into directory and filename
            var directoryPath = Path.GetDirectoryName(pathWithinShare).Replace("\\", "/"); 
            var fileName = Path.GetFileName(pathWithinShare);

            // Ensure directory exists
            var directoryClient = _shareClient.GetDirectoryClient(directoryPath);
            await directoryClient.CreateIfNotExistsAsync();

            // Get file client and upload
            var fileClient = directoryClient.GetFileClient(fileName);
            await fileClient.CreateAsync(content.Length);
            await fileClient.UploadAsync(content);
        }

        private async Task EnsureDirectoriesExist(ShareDirectoryClient dirClient)
        {
            // 
            try
            {
                await dirClient.CreateIfNotExistsAsync();
            }
            catch
            {
                // ignore
            }
        }
    }
}
