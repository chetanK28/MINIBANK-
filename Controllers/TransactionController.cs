using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MINIBANK.Models;
using MINIBANK.Services.Implementation;
using MINIBANK.Services.Interface;

namespace MINIBANK.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionService transactionService;
        private readonly IBankAccountService bankAccountService;

        // Constructor with dependency injection
        public TransactionController(ITransactionService transactionService, IBankAccountService bankAccountService)
        {
            this.transactionService = transactionService;
            this.bankAccountService = bankAccountService;
        }

        // ✅ View All Transactions
        public IActionResult ViewAllTransactions()
        {
            var transactions = transactionService.GetAllTransactionsService();
            if (transactions == null || !transactions.Any())
            {
                ViewBag.Message = "No transactions found.";
            }
            return View(transactions);
        }

        // ✅ View Transactions by Account ID
        public IActionResult ViewTransactionsByAccountId(long accountId)
        {
            var transactions = transactionService.GetTransactionsByAccountIdService(accountId);
            if (transactions == null || !transactions.Any())
            {
                ViewBag.Message = "No transactions found for the selected account.";
            }
            return View(transactions);
        }

        // ✅ View Transactions by User ID
        public IActionResult ViewTransactionsByUserId(int userId)
        {
            var transactions = transactionService.GetTransactionsByUserIdService(userId);
            if (transactions == null || !transactions.Any())
            {
                ViewBag.Message = "No transactions found for the selected user.";
            }
            return View(transactions);
        }

        // ✅ Add Transaction (GET)
        [HttpGet]
        public IActionResult AddTransaction()
        {
            
            ViewData["Title"] = "Add New Transaction";

            // Fetch all accounts to populate the dropdown
            var bankAccounts = bankAccountService.GetBankAccounts();
            if (bankAccounts == null || !bankAccounts.Any())
            {
                throw new Exception("No bank accounts found. Please ensure accounts exist before adding a transaction.");
            }

            ViewBag.AccountId = new SelectList(bankAccounts, "AccountId", "AccountId");

            return View();
        }

        // ✅ Add Transaction (POST)
        [HttpPost]
        public IActionResult AddTransaction([Bind(include: "AccountId,Amount,Type,RelatedAccountId")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                try
                {
                        transactionService.CreateTransactionService(transaction);
                    return RedirectToAction("ViewAllTransactions");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while adding the transaction. Please try again.");
                }
            }
            foreach (var modelStateKey in ModelState.Keys)
            {
                var value = ModelState[modelStateKey];
                foreach (var error in value.Errors)
                {
                    Console.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                }
            }



            // Repopulate the dropdown in case of validation errors
            var bankAccounts = bankAccountService.GetBankAccounts();
            ViewBag.AccountId = new SelectList(bankAccounts, "AccountId", "AccountId", transaction.AccountId);

            return View(transaction);
        }

        // ✅ View Transaction Details by ID
        public IActionResult ViewTransactionById(long transactionId)
        {
            var transaction = transactionService.GetTransactionByIdService(transactionId);
            if (transaction == null)
            {
                return NotFound("Transaction not found.");
            }
            return View(transaction);
        }

        // ✅ Delete Transaction (GET) — Confirmation Page
        // ✅ Delete Transaction (GET) — Confirmation Page
        [HttpGet]
        public IActionResult DeleteTransaction(long transactionId)
        {
            var transaction = transactionService.GetTransactionByIdService(transactionId);
            if (transaction == null)
            {
                return NotFound("Transaction not found.");
            }

            return View(transaction); // Show the confirmation page with transaction details
        }

        // ✅ Delete Transaction (POST)
        [HttpPost, ActionName("DeleteTransaction")]
        public IActionResult DeleteTransactionConfirmed(long transactionId)
        {
            try
            {
                transactionService.DeleteTransactionService(transactionId); // Call the service to delete the transaction
                TempData["SuccessMessage"] = "Transaction deleted successfully.";
                return RedirectToAction("ViewAllTransactions");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while deleting the transaction. Please try again.";
            }

            return RedirectToAction("ViewAllTransactions");
        }
    }
}
