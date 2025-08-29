using Azure.Data.Tables;
using Microsoft.Extensions.Options;
using AbcRetail.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;

namespace AbcRetail.Services
{
    public interface IProductTableService
    {
        Task<IEnumerable<ProductEntity>> GetAllProductsAsync();
        Task<ProductEntity> GetProductByIdAsync(string rowKey);
        Task AddProductAsync(ProductEntity product);
        Task UpdateProductAsync(ProductEntity product);
        Task DeleteProductAsync(string rowKey);
    }
}
namespace AbcRetail.Services
{
    public class ProductTableService : IProductTableService
    {

        private readonly TableClient _tableClient;

        public ProductTableService(IOptions<AzureStorageOptions> options)
        {
            var connectionString = options.Value.ConnectionString;
            var tableName = "Products";

            _tableClient = new TableClient(connectionString, tableName);
            _tableClient.CreateIfNotExists(); // Ensure table exists
        }

        public async Task<IEnumerable<ProductEntity>> GetAllProductsAsync()
        {
            var results = _tableClient.QueryAsync<ProductEntity>(p => p.PartitionKey == "PRODUCTS");
            var list = new List<ProductEntity>();
            await foreach (var item in results)
            {
                list.Add(item);
            }
            return list;
        }

        public async Task<ProductEntity> GetProductByIdAsync(string rowKey)
        {
            try
            {
                var response = await _tableClient.GetEntityAsync<ProductEntity>("PRODUCTS", rowKey);
                return response.Value;
            }
            catch (Azure.RequestFailedException)
            {
                return null;
            }
        }

        public async Task AddProductAsync(ProductEntity product)
        {
            if (string.IsNullOrEmpty(product.PartitionKey))
                product.PartitionKey = "PRODUCTS";
            if (string.IsNullOrEmpty(product.RowKey))
                product.RowKey = Guid.NewGuid().ToString();

            await _tableClient.AddEntityAsync(product);
        }

        public async Task UpdateProductAsync(ProductEntity product)
        {
            await _tableClient.UpdateEntityAsync(product, ETag.All);
        }

        public async Task DeleteProductAsync(string rowKey)
        {
            await _tableClient.DeleteEntityAsync("PRODUCTS", rowKey);
        }
    }
}
