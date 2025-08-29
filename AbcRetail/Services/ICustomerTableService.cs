using AbcRetail.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbcRetail.Services
{
    public interface ICustomerTableService
    {
        Task<List<CustomerEntity>> GetAllCustomersAsync();
        Task<CustomerEntity> GetCustomerAsync(string partitionKey, string rowKey);
        Task AddCustomerAsync(CustomerEntity customer);
        Task UpdateCustomerAsync(CustomerEntity customer);
        Task DeleteCustomerAsync(string partitionKey, string rowKey);
    }
}
