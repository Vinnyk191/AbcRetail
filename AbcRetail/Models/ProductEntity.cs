using Azure;
using Azure.Data.Tables;

namespace AbcRetail.Models
{
    public class ProductEntity : ITableEntity
    {
        public string PartitionKey { get; set; } = "PRODUCT";
        public string RowKey { get; set; } = Guid.NewGuid().ToString();  // product ID
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
    }
}
