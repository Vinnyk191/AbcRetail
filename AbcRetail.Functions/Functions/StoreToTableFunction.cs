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
    public class StoreToTableFunction
    {
        private readonly ICustomerTableService _customers;
        private readonly IProductTableService _products;
        private readonly ILogger<StoreToTableFunction> _logger;

        public StoreToTableFunction(ICustomerTableService customers, IProductTableService products, ILogger<StoreToTableFunction> logger)
        {
            _customers = customers;
            _products = products;
            _logger = logger;
        }

        [FunctionName("StoreToTable")]
        public async Task<IActionResult> Run(
            [Microsoft.Azure.Functions.Worker.HttpTrigger(AuthorizationLevel.Function, "post", Route = "store/{entityType}")] HttpRequest req,
            string entityType)
        {
            // Read JSON body
            var json = await new System.IO.StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(json))
                return new BadRequestObjectResult("Empty body");

            if (entityType == "customer")
            {
                var customer = JsonSerializer.Deserialize<CustomerEntity>(json);
                if (customer == null) return new BadRequestObjectResult("Invalid customer data");
                await _customers.AddCustomerAsync(customer);
                _logger.LogInformation("Customer stored: {id}", customer.RowKey);
                return new OkObjectResult(new { message = "Customer stored", id = customer.RowKey });
            }
            else if (entityType == "product")
            {
                var product = JsonSerializer.Deserialize<ProductEntity>(json);
                if (product == null) return new BadRequestObjectResult("Invalid product data");
                await _products.AddProductAsync(product);
                _logger.LogInformation("Product stored: {id}", product.RowKey);
                return new OkObjectResult(new { message = "Product stored", id = product.RowKey });
            }
            return new BadRequestObjectResult("Unknown entityType");
        }
    }
}
