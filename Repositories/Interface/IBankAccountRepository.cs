using MINIBANK.Models;

namespace MINIBANK.Repositories.Interface
{
    public interface IBankAccountRepository
    {
        void CreateBankAccount(BankAccount bankAccount);
        void UpdateBankAccount(BankAccount bankAccount);
        void DeleteBankAccount(long accountId);
        BankAccount GetBankAccountById(long accountId);
        List<BankAccount> GetBankAccountsByUserId(int userId);
        List<BankAccount> GetBankAccounts();

    }
}
