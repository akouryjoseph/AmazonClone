using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AmazonClone.Api.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        public int CartId { get; set; }

        [JsonIgnore]
        public Cart Cart { get; set; } = null!;

        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
    }
}