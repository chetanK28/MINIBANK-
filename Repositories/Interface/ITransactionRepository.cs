using MINIBANK.Models;

namespace MINIBANK.Repositories.Interface
{
    public interface ITransactionRepository
    {
        void CreateTransaction(Transaction transaction);
        Transaction GetTransactionById(long transactionId);
        List<Transaction> GetTransactionsByAccountId(long accountId);
        List<Transaction> GetTransactionsByUserId(int userId);
        List<Transaction> GetAllTransactions();
        void DeleteTransaction(long transactionId);
    }
}
