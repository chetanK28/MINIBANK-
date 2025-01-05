using Microsoft.EntityFrameworkCore;
using MINIBANK.DBContext;
using MINIBANK.Models;
using MINIBANK.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MINIBANK.Repositories.Implementation
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly Context _context;

        // Constructor with dependency injection
        public TransactionRepository(Context context)
        {
            _context = context;
        }

        // ✅ Create a Transaction
        public void CreateTransaction(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction), "Transaction object cannot be null.");
            }

            // Check if the account exists
            var account = _context.BankAccount.Find(transaction.AccountId);
            if (account == null)
            {
                throw new Exception("Bank account not found. Cannot create transaction.");
            }

            // For transfers, check if the related account exists
            if (transaction.Type == TransactionType.Transfer && transaction.RelatedAccountId != null)
            {
                var relatedAccount = _context.BankAccount.Find(transaction.RelatedAccountId);
                if (relatedAccount == null)
                {
                    throw new Exception("Related bank account not found for transfer.");
                }
            }

            // Add the transaction
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }

        // ✅ Get Transaction by ID
        public Transaction GetTransactionById(long transactionId)
        {
            var transaction = _context.Transactions
                .Include(t => t.Account)
                .Include(t => t.RelatedAccount)
                .FirstOrDefault(t => t.TransactionId == transactionId);

            if (transaction == null)
            {
                throw new Exception("Transaction not found.");
            }

            return transaction;
        }

        // ✅ Get Transactions by Account ID
        public List<Transaction> GetTransactionsByAccountId(long accountId)
        {
            if (!_context.BankAccount.Any(b => b.AccountId == accountId))
            {
                throw new Exception("Bank account not found. Cannot retrieve transactions.");
            }

            return _context.Transactions
                .Where(t => t.AccountId == accountId)
                .Include(t => t.Account)
                .OrderByDescending(t => t.CreatedAt)
                .ToList();
        }

        // ✅ Get Transactions by User ID
        public List<Transaction> GetTransactionsByUserId(int userId)
        {
            if (!_context.Users.Any(u => u.Id == userId))
            {
                throw new Exception("User not found. Cannot retrieve transactions.");
            }

            return _context.Transactions
                .Where(t => _context.BankAccount.Any(b => b.AccountId == t.AccountId && b.UserId == userId))
                .Include(t => t.Account)
                .OrderByDescending(t => t.CreatedAt)
                .ToList();
        }

        // ✅ Get All Transactions
        public List<Transaction> GetAllTransactions()
        {
            return _context.Transactions
                .Include(t => t.Account)
                .Include(t => t.RelatedAccount)
                .OrderByDescending(t => t.CreatedAt)
                .ToList();
        }

        // ✅ Delete Transaction
        public void DeleteTransaction(long transactionId)
        {
            var transaction = _context.Transactions.Find(transactionId);
            if (transaction == null)
            {
                throw new Exception("Transaction not found.");
            }

            _context.Transactions.Remove(transaction);
            _context.SaveChanges();
        }


    }
}
