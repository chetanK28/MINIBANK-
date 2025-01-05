using Microsoft.EntityFrameworkCore;
using MINIBANK.DBContext;
using MINIBANK.Models;
using MINIBANK.Repositories.Interface;
using System.Collections.Generic;
using System.Linq;

namespace MINIBANK.Repositories.Implementation
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly Context _context;

        public BankAccountRepository(Context context)
        {
            _context = context;
        }

        // ✅ Create a Bank Account
        public void CreateBankAccount(BankAccount bankAccount)
        {
            var user = _context.Users.Find(bankAccount.UserId);
            if (user == null)
            {
                throw new Exception("User not found. Cannot create bank account without a valid user.");
            }

            _context.BankAccount.Add(bankAccount);
            _context.SaveChanges();
        }

        // ✅ Delete a Bank Account
        public void DeleteBankAccount(long accountId)
        {
            var bankAccount = _context.BankAccount.Find(accountId);
            if (bankAccount == null)
            {
                throw new Exception("Bank account not found. Cannot delete.");
            }

            _context.BankAccount.Remove(bankAccount);
            _context.SaveChanges();
        }

        // ✅ Get Bank Account by ID
        public BankAccount GetBankAccountById(long accountId)
        {
            var bankAccount = _context.BankAccount
                .Include(b => b.User)
                .FirstOrDefault(b => b.AccountId == accountId);

            if (bankAccount == null)
            {
                throw new Exception("Bank account not found.");
            }

            return bankAccount;
        }

        // ✅ Get All Bank Accounts
        public List<BankAccount> GetBankAccounts()
        {
            return _context.BankAccount
                .Include(b => b.User)
                .ToList();
        }

        // ✅ Get Bank Accounts by User ID
        public List<BankAccount> GetBankAccountsByUserId(int userId)
        {
            if (!_context.Users.Any(u => u.Id == userId))
            {
                throw new Exception("User not found. Cannot retrieve bank accounts.");
            }

            return _context.BankAccount
                .Include(b => b.User)
                .Where(b => b.UserId == userId)
                .ToList();
        }

        // ✅ Update Bank Account
        public void UpdateBankAccount(BankAccount bankAccount)
        {
            var existingAccount = _context.BankAccount.Find(bankAccount.AccountId);
            if (existingAccount == null)
            {
                throw new Exception("Bank account not found. Cannot update.");
            }

            // Update fields
            existingAccount.Balance = bankAccount.Balance;
            existingAccount.Type = bankAccount.Type;

            _context.SaveChanges();
        }
    }
}
