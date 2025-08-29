namespace AbcRetail.Services
{
    public interface IQueueService
    {
        Task EnsureQueueExistsAsync();
        Task EnqueueMessageAsync(string messageText);
        Task<List<string>> GetOrderMessagesAsync();
        Task DeleteMessageAsync(string messageId);
    }
}
