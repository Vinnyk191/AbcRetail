using Azure.Data.Tables;

namespace AbcRetail.Models
{
    public class AzureStorageOptions
    {

        public string ConnectionString { get; set; } = "";
        public string FileShareName { get; set; } = "logs";
        public string TableNameCustomers { get; set; } = "Customers";
        public string TableNameProducts { get; set; } = "Products";
        public string QueueName { get; set; } = "orders";
        public string BlobContainer { get; set; } = "product-images";
    }
}
