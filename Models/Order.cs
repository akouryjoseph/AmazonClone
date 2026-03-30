namespace AmazonClone.Api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public List<OrderItem> Items { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
    
}