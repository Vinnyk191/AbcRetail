namespace AbcRetail.Models
{
    public class OrderMessage
    {
        public string OrderId { get; set; } = Guid.NewGuid().ToString();
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Notes { get; set; }
    }
}
