using AbcRetail.Models;

namespace AbcRetail.Services
{
    public interface ITableStorageService
    {
        Task EnsureTablesCreatedAsync();
        Task UpsertCustomerAsync(CustomerEntity customer);
        Task UpsertProductAsync(ProductEntity product);
        Task<IEnumerable<ProductEntity>> GetAllProductsAsync();
        Task<CustomerEntity> GetCustomerAsync(string customerId);
        Task<ProductEntity> GetProductAsync(string productId);
    }
}
