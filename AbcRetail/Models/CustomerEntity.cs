using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;

namespace AbcRetail.Models
{
    public class CustomerEntity : ITableEntity
    {
        public string PartitionKey { get; set; } = "Customer";
        public string RowKey { get; set; }  = Guid.NewGuid().ToString();
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Customer Identity
        [Required]
        public string FullName { get; set; }
        public string Email {  get; set; }
        public string Phone { get; set; }
        //public DateTime CreatedAt { get; set; }
        public string AddressJson { get; set; } // optional
    }
}
