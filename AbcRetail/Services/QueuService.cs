using AbcRetail.Models;
using AbcRetail.Services;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AbcRetail.Services
{
    public class QueueService : IQueueService
    {
        private readonly QueueClient _queueClient;
        private readonly AzureStorageOptions _opts;

        /*public QueueService(IOptions<AzureStorageOptions> options)
        {
            _opts = options.Value;
            var client = new QueueServiceClient(_opts.ConnectionString);
            _queueClient = client.GetQueueClient(_opts.QueueOrders);
            EnsureQueueExistsAsync().GetAwaiter().GetResult();
        }*/

        //
        public QueueService(IOptions<AzureStorageOptions> options)
        {
            _queueClient = new QueueClient(options.Value.ConnectionString, "orders"); // "orders" is the queue name
            _queueClient.CreateIfNotExists();
        }

        public async Task<List<string>> GetOrderMessagesAsync()
        {
            var messagesList = new List<string>();

            QueueMessage[] messages = await _queueClient.ReceiveMessagesAsync(maxMessages: 20);

            foreach (var msg in messages)
            {
                messagesList.Add(msg.MessageText);

                
            }

            return messagesList;
        }

        //
        public async Task DeleteMessageAsync(string messageId)
        {
            // Implement deletion logic if needed
        }

        //

        public async Task EnsureQueueExistsAsync()
        {
            await _queueClient.CreateIfNotExistsAsync();
        }

        public async Task EnqueueMessageAsync(string messageText)
        {
            if (string.IsNullOrEmpty(messageText)) throw new ArgumentNullException(nameof(messageText));
            await _queueClient.SendMessageAsync(messageText);
        }
    }
}