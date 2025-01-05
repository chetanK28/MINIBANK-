using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MINIBANK.Models
{
    public enum TransactionType
    {
        Deposit = 1,
        Withdraw = 2,
        Transfer = 3
    }

    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TransactionId { get; set; }

        [Required]
        [ForeignKey("Account")]
        public long AccountId { get; set; }

        public virtual BankAccount? Account { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public double Amount { get; set; }

        [Required]
        public TransactionType Type { get; set; }

        [ForeignKey("RelatedAccount")]
        public long? RelatedAccountId { get; set; }

        public virtual BankAccount? RelatedAccount { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Transaction() { }

        public Transaction(long accountId, double amount, TransactionType type, long? relatedAccountId = null)
        {
            AccountId = accountId;
            Amount = amount;
            Type = type;
            RelatedAccountId = relatedAccountId;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
