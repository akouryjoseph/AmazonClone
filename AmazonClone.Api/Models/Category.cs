// Models/Category.cs
namespace AmazonClone.Api.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        // Navigation property to products
        public List<Product> Products { get; set; } = new();
    }
}
