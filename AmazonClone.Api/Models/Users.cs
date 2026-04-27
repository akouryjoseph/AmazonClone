namespace AmazonClone.Api.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;

        // NEVER store plain passwords
        public string PasswordHash { get; set; } = null!;

        // User or Admin
        public string Role { get; set; } = "User";
    }
}
