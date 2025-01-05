using System.ComponentModel.DataAnnotations;

namespace MINIBANK.Models
{
    public enum UserRole
    {
        Admin,
        User,
        Moderator
    }
    public class Users
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }  
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        
        public UserRole Role { get; set; }
        public ICollection<BankAccount> BankAccount { get; set; } = new List<BankAccount>();

    }
}
