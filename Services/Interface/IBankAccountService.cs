using MINIBANK.Models;

namespace MINIBANK.Services.Interface
{
    public interface IBankAccountService
    {
        public void CreateBankAccount(BankAccount bankAccount);
        public void UpdateBankAccount(BankAccount bankAccount);
        public void DeleteBankAccount(long accountId);
        public BankAccount GetBankAccountById(long accountId);
        public List<BankAccount> GetBankAccounts();
        public List<BankAccount> GetBankAccountsByUserId(int userId);


    }
}
