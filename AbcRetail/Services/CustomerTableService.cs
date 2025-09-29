using Azure.Data.Tables;
using AbcRetail.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Azure;

/*namespace AbcRetail.Services
{
    public interface ICustomerTableService
    {
        Task<List<Customer>> GetCustomersAsync();
        Task<Customer?> GetCustomerAsync( string rowKey);
        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(string partitionKey, string rowKey);
    }

    public class CustomerTableService : ICustomerTableService
    {
        private readonly TableClient _tableClient;

        public CustomerTableService(IConfiguration config)
        {
            var connectionString = config["AzureStorage:ConnectionString"];
            var tableName = "Customers";
            _tableClient = new TableClient(connectionString, tableName);
            _tableClient.CreateIfNotExists();
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            var customers = new List<Customer>();

            
            await foreach (var c in _tableClient.QueryAsync<Customer>(c => c.PartitionKey == "Customers"))
            {
                customers.Add(c);
            }

            return customers;
        }

        public async Task<Customer?> GetCustomerAsync(string rowKey)
        {
            try
            {
                
                var response = await _tableClient.GetEntityAsync<Customer>("Customers", rowKey);
                return response.Value;
            }
            catch
            {
                return null;
            }
        }


        public async Task AddCustomerAsync(Customer customer)
        {
            await _tableClient.AddEntityAsync(customer);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            await _tableClient.UpdateEntityAsync(customer, customer.ETag, TableUpdateMode.Replace);
        }

        /*public async Task DeleteCustomerAsync(string rowKey)
        {
            await _tableClient.DeleteEntityAsync("Customers", rowKey);
        }*/

/*  public async Task DeleteCustomerAsync(string partitionKey, string rowKey)
  {
      await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
  }
}
}
*/

//
namespace AbcRetail.Services
{
    public class CustomerTableService : ICustomerTableService
    {
        private readonly TableClient _tableClient;

        public CustomerTableService(IOptions<AzureStorageOptions> options)
        {
            var tableName = options.Value.TableNameCustomers;
            var connectionString = options.Value.ConnectionString;

            _tableClient = new TableClient(connectionString, tableName);
            _tableClient.CreateIfNotExists();
        }

        public async Task<List<CustomerEntity>> GetAllCustomersAsync()
        {
            var customers = _tableClient.QueryAsync<CustomerEntity>();
            var list = new List<CustomerEntity>();

            await foreach (var customer in customers)
            {
                list.Add(customer);
            }

            return list;
        }

        public async Task AddCustomerAsync(CustomerEntity customer)
        {
            customer.RowKey = customer.RowKey ?? Guid.NewGuid().ToString();
            customer.PartitionKey = "CUSTOMERS";
            await _tableClient.AddEntityAsync(customer);
        }

        public async Task UpdateCustomerAsync(CustomerEntity customer)
        {
            await _tableClient.UpdateEntityAsync(customer, customer.ETag);
        }

        public async Task<CustomerEntity> GetCustomerAsync(string partitionKey, string rowKey)
        {
            var response = await _tableClient.GetEntityAsync<CustomerEntity>(partitionKey, rowKey);
            return response.Value;
        }

        public async Task DeleteCustomerAsync(string partitionKey, string rowKey)
        {
            await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }
    }
}
