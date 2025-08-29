using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AbcRetail.Models;
using Azure.Storage.Queues.Models;
using Azure.Storage.Queues;
using Microsoft.Extensions.Options;

namespace AbcRetail.Services
{
    public interface IOrderQueueService
    {
        Task EnqueueOrderAsync(OrderMessage orderMessage);
        Task<List<OrderMessage>> GetAllOrdersAsync();
    }
}

namespace AbcRetail.Services
{
    public class OrderQueueService : IOrderQueueService
    {
        private readonly QueueClient _queueClient;

        public OrderQueueService(IOptions<AzureStorageOptions> options)
        {
            // Connect to Azure Queue
            var connectionString = options.Value.ConnectionString;
            var queueName = "orders";

            _queueClient = new QueueClient(connectionString, queueName);
            _queueClient.CreateIfNotExists();
        }

        // Add order to the queue
         public async Task EnqueueOrderAsync(OrderMessage orderMessage)
         {
             if (orderMessage == null)
                 throw new ArgumentNullException(nameof(orderMessage));

             var messageJson = JsonSerializer.Serialize(orderMessage);
             await _queueClient.SendMessageAsync(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(messageJson)));
         }

         // Retrieve all messages currently in the queue
         public async Task<List<OrderMessage>> GetAllOrdersAsync()
         {
             var orders = new List<OrderMessage>();

             if (!_queueClient.Exists())
                 return orders;

             // Peek at messages (does not remove them)
             QueueProperties properties = await _queueClient.GetPropertiesAsync();
             int messagesToPeek = properties.ApproximateMessagesCount > 32 ? 32 : properties.ApproximateMessagesCount;

             if (messagesToPeek == 0)
                 return orders;

             PeekedMessage[] peekedMessages = await _queueClient.PeekMessagesAsync(messagesToPeek);

             foreach (var msg in peekedMessages)
             {
                 var json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(msg.MessageText));
                 var order = JsonSerializer.Deserialize<OrderMessage>(json);
                 orders.Add(order);
             }

             return orders;
         }

        
    }
}
