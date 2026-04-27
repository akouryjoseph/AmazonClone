using System.ComponentModel.DataAnnotations;

namespace AmazonClone.Api.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;

        // Add category reference
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
