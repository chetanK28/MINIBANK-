using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MINIBANK.Models
{
    public enum AccountType
    {
        Savings,
        Current,
        FixedDeposit
    }
    public class BankAccount
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AccountId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Balance is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Balance cannot be negative.")]
        public double Balance { get; set; }


        [Required]
        public AccountType Type { get; set; }

        [Required(ErrorMessage = "CreatedAt is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid datetime format.")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        // Navigation Property
        public Users? User { get; set; }

        public BankAccount()
        {

        }
        public BankAccount(long accountId, int userId, double balance, AccountType type)
        {
            AccountId = accountId;
            UserId = userId;
            Balance = balance;
            Type = type;
            CreatedAt = DateTime.Now;
        }


    }
}
