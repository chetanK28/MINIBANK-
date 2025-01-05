using MINIBANK.Models;
using MINIBANK.Repositories.Interface;
using MINIBANK.Services.Interface;

namespace MINIBANK.Services.Implementation
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IBankAccountRepository bankAccountRepository;

        // Constructor Dependency Injection
        public TransactionService(ITransactionRepository transactionRepository, IBankAccountRepository bankAccountRepository)
        {
            this.transactionRepository = transactionRepository;
            this.bankAccountRepository = bankAccountRepository;
        }

        // ✅ Create Transaction
        public void CreateTransactionService(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction), "Transaction object cannot be null.");
            }

            var account = bankAccountRepository.GetBankAccountById(transaction.AccountId);
            if (account == null)
            {
                throw new ArgumentException("Account not found. Cannot create transaction.");
            }

            // Handle specific logic for transaction types
            switch (transaction.Type)
            {
                case TransactionType.Deposit:
                    account.Balance += transaction.Amount;
                    break;

                case TransactionType.Withdraw:
                    if (account.Balance < transaction.Amount)
                    {
                        throw new InvalidOperationException("Insufficient balance for withdrawal.");
                    }
                    account.Balance -= transaction.Amount;
                    break;

                case TransactionType.Transfer:
                    if (transaction.RelatedAccountId == null)
                    {
                        throw new ArgumentException("RelatedAccountId is required for transfer transactions.");
                    }

                    var relatedAccount = bankAccountRepository.GetBankAccountById(transaction.RelatedAccountId.Value);
                    if (relatedAccount == null)
                    {
                        throw new ArgumentException("Recipient account not found for transfer.");
                    }

                    if (account.Balance < transaction.Amount)
                    {
                        throw new InvalidOperationException("Insufficient balance for transfer.");
                    }

                    account.Balance -= transaction.Amount;
                    relatedAccount.Balance += transaction.Amount;

                    bankAccountRepository.UpdateBankAccount(relatedAccount);
                    break;

                default:
                    throw new ArgumentException("Invalid transaction type.");
            }

            bankAccountRepository.UpdateBankAccount(account);
            transactionRepository.CreateTransaction(transaction);
        }

        // ✅ Get Transaction by ID
        public Transaction GetTransactionByIdService(long transactionId)
        {
            var transaction = transactionRepository.GetTransactionById(transactionId);
            if (transaction == null)
            {
                throw new Exception("Transaction not found.");
            }
            return transaction;
        }

        // ✅ Get Transactions by Account ID
        public List<Transaction> GetTransactionsByAccountIdService(long accountId)
        {
            if (accountId <= 0)
            {
                throw new ArgumentException("Invalid account ID.");
            }

            return transactionRepository.GetTransactionsByAccountId(accountId);
        }

        // ✅ Get Transactions by User ID
        public List<Transaction> GetTransactionsByUserIdService(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID.");
            }

            return transactionRepository.GetTransactionsByUserId(userId);
        }

        // ✅ Get All Transactions
        public List<Transaction> GetAllTransactionsService()
        {
            var transactions = transactionRepository.GetAllTransactions();
            if (transactions == null || !transactions.Any())
            {
                throw new Exception("No transactions found.");
            }

            return transactions;
        }

        // ✅ Delete Transaction
        public void DeleteTransactionService(long transactionId)
        {
            if (transactionId <= 0)
            {
                throw new ArgumentException("Invalid transaction ID.");
            }

            var transaction = transactionRepository.GetTransactionById(transactionId);
            if (transaction == null)
            {
                throw new Exception("Transaction not found. Cannot delete.");
            }

            transactionRepository.DeleteTransaction(transactionId);
        }
    }
}
