using MINIBANK.Models;
using MINIBANK.Repositories.Interface;
using MINIBANK.Services.Interface;

namespace MINIBANK.Services.Implementation
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository bankAccountRepository;

        // ✅ Constructor Dependency Injection
        public BankAccountService(IBankAccountRepository bankAccountRepository)
        {
            this.bankAccountRepository = bankAccountRepository;
        }

        // ✅ Create Bank Account
        public void CreateBankAccount(BankAccount bankAccount)
        {
            if (bankAccount == null)
            {
                throw new ArgumentNullException(nameof(bankAccount), "Bank account object cannot be null.");
            }
            bankAccountRepository.CreateBankAccount(bankAccount);
        }

        // ✅ Delete Bank Account
        public void DeleteBankAccount(long accountId)
        {
            if (accountId <= 0)
            {
                throw new ArgumentException("Invalid account ID provided.");
            }
            bankAccountRepository.DeleteBankAccount(accountId);
        }

        // ✅ Get Bank Account by ID
        public BankAccount GetBankAccountById(long accountId)
        {
            if (accountId <= 0)
            {
                throw new ArgumentException("Invalid account ID provided.");
            }
            return bankAccountRepository.GetBankAccountById(accountId);
        }

        // ✅ Get All Bank Accounts
        public List<BankAccount> GetBankAccounts()
        {
            return bankAccountRepository.GetBankAccounts();
        }

        // ✅ Get Bank Accounts by User ID
        public List<BankAccount> GetBankAccountsByUserId(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID provided.");
            }
            return bankAccountRepository.GetBankAccountsByUserId(userId);
        }

        // ✅ Update Bank Account
        public void UpdateBankAccount(BankAccount bankAccount)
        {
            if (bankAccount == null || bankAccount.AccountId <= 0)
            {
                throw new ArgumentException("Invalid bank account object or AccountId.");
            }
            bankAccountRepository.UpdateBankAccount(bankAccount);
        }
    }
}
