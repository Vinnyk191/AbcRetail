namespace AbcRetail.Services
{
    public interface IFileShareService
    {
        Task EnsureShareExistsAsync();
        Task UploadLogFileAsync(string pathWithinShare, Stream content);
        Task<List<string>> GetLogFilesAsync();
    }
}
