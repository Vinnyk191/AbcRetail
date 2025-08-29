using System.ComponentModel.DataAnnotations;

namespace AbcRetail.Models
{
    public class OrderEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        

        [Required]
        public string CustomerId { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}
