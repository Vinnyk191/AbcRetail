using AbcRetail.Models;
using AbcRetail.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AbcRetail.Functions.Functions
{
    public class EnqueueTransactionFunction
    {
        private readonly IOrderQueueService _queueService;
        private readonly ILogger<EnqueueTransactionFunction> _logger;

        public EnqueueTransactionFunction(IOrderQueueService queueService, ILogger<EnqueueTransactionFunction> logger)
        {
            _queueService = queueService;
            _logger = logger;
        }

        [FunctionName("EnqueueTransaction")]
        public async Task<IActionResult> Enqueue(
            [Microsoft.Azure.Functions.Worker.HttpTrigger(AuthorizationLevel.Function, "post", Route = "queue/enqueue")] HttpRequest req)
        {
            var json = await new System.IO.StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(json)) return new BadRequestObjectResult("Empty body");

            var message = JsonSerializer.Deserialize<OrderMessage>(json);
            if (message == null) return new BadRequestObjectResult("Invalid order message");

            await _queueService.EnqueueOrderAsync(message);
            _logger.LogInformation("Enqueued order {id}", message.OrderId);
            return new OkObjectResult(new { message = "enqueued", id = message.OrderId });
        }

        [FunctionName("GetTransactions")]
        public async Task<IActionResult> GetTransactions(
            [Microsoft.Azure.Functions.Worker.HttpTrigger(AuthorizationLevel.Function, "get", Route = "queue/peek")] HttpRequest req)
        {
            var messages = await _queueService.GetAllOrdersAsync();
            return new OkObjectResult(messages);
        }
    }
}
