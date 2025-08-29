namespace AbcRetail.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(Stream content, string fileName, string contentType = null);
    }
}
