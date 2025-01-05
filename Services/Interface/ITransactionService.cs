using MINIBANK.Models;

namespace MINIBANK.Services.Interface
{
    public interface ITransactionService
    {
        void CreateTransactionService(Transaction transaction);
        Transaction GetTransactionByIdService(long transactionId);
        List<Transaction> GetTransactionsByAccountIdService(long accountId);
        List<Transaction> GetTransactionsByUserIdService(int userId);
        List<Transaction> GetAllTransactionsService();
        void DeleteTransactionService(long transactionId);
    }
}
