using Azure;
using Azure.Data.Tables;
using AbcRetail.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbcRetail.Services;

namespace AbcRetailServices
{
    public class TableStorageService : ITableStorageService
    {
        private readonly TableClient _customersTable;
        private readonly TableClient _productsTable;
        private readonly AzureStorageOptions _opts;

        public TableStorageService(IOptions<AzureStorageOptions> options)
        {
            _opts = options.Value;
            var conn = _opts.ConnectionString;
            _customersTable = new TableClient(conn, _opts.TableCustomers);
            _productsTable = new TableClient(conn, _opts.TableProducts);

            EnsureTablesCreatedAsync().GetAwaiter().GetResult();
        }

        public async Task EnsureTablesCreatedAsync()
        {
            await _customersTable.CreateIfNotExistsAsync();
            await _productsTable.CreateIfNotExistsAsync();
        }

        public async Task UpsertCustomerAsync(CustomerEntity customer)
        {
            if (string.IsNullOrEmpty(customer.RowKey))
                customer.RowKey = System.Guid.NewGuid().ToString();
            await _customersTable.UpsertEntityAsync(customer);
        }

        public async Task UpsertProductAsync(ProductEntity product)
        {
            if (string.IsNullOrEmpty(product.RowKey))
                product.RowKey = System.Guid.NewGuid().ToString();
            await _productsTable.UpsertEntityAsync(product);
        }

        public async Task<IEnumerable<ProductEntity>> GetAllProductsAsync()
        {
            var q = _productsTable.QueryAsync<ProductEntity>();
            var result = new List<ProductEntity>();
            await foreach (var item in q)
            {
                result.Add(item);
            }
            return result;
        }

        public async Task<CustomerEntity> GetCustomerAsync(string customerId)
        {
            try
            {
                var resp = await _customersTable.GetEntityAsync<CustomerEntity>("CUSTOMER", customerId);
                return resp.Value;
            }
            catch (RequestFailedException)
            {
                return null;
            }
        }

        public async Task<ProductEntity> GetProductAsync(string productId)
        {
            try
            {
                var resp = await _productsTable.GetEntityAsync<ProductEntity>("PRODUCT", productId);
                return resp.Value;
            }
            catch (RequestFailedException)
            {
                return null;
            }
        }
    }
}
