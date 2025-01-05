using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MINIBANK.DBContext;
using MINIBANK.Models;
using MINIBANK.Services.Implementation;
using MINIBANK.Services.Interface;

namespace MINIBANK.Controllers
{
        
    public class BankAccountController : Controller
    {
        private readonly IBankAccountService bankAccountService;
        private readonly IUserService userService;
        private Context context = new Context();

        public BankAccountController(IBankAccountService bankAccountService,IUserService userService)
        {
            this.bankAccountService = bankAccountService;
            this.userService = userService;
        }

        // ✅ Display All Bank Accounts
       
        public IActionResult ViewBankAccounts()
        {
            var bankAccounts = bankAccountService.GetBankAccounts();
            return View(bankAccounts);
        }

        // ✅ Display Bank Accounts by UserId
        public IActionResult ViewBankAccountsByUserId(int userId)
        {
            if (userId <= 0)
            {
                Console.WriteLine($"Invalid User ID: {userId}");
                throw new ArgumentException("Invalid user ID provided.");
            }

            var accounts = bankAccountService.GetBankAccountsByUserId(userId);

            if (accounts == null || accounts.Count == 0)
            {
                Console.WriteLine($"No bank accounts found for User ID: {userId}");
            }

            return View(accounts);
        }

        // ✅ Display Bank Account Details by Id
        public IActionResult ViewBankAccountById(long accountId)
        {
            var bankAccount = bankAccountService.GetBankAccountById(accountId);
            if (bankAccount == null)
            {
                return NotFound("Bank account not found");
            }
            return View(bankAccount);
        }

        // ✅ Add Bank Account (GET)
        //[Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult AddBankAccount()
        {
            ViewData["Title"] = "Add New Bank Account";
            var users = userService.GetAllUsers();
            if (users == null || !users.Any())
            {
                throw new Exception("No users found. Please ensure the database contains user records.");
            }

            IEnumerable<Users> users1 = userService.GetAllUsers();

            ViewBag.UserId = new SelectList(users1, "Id", "Id");

            return View();
        }


        // ✅ Add Bank Account (POST)
        
        [HttpPost]
        public IActionResult AddBankAccount( [Bind(include:"AccountId,UserId,Balance,Type,CreatedAt")] BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                bankAccountService.CreateBankAccount(bankAccount);
                return RedirectToAction("ViewBankAccounts");
            }

            // Debug Validation Errors
            foreach (var modelStateKey in ModelState.Keys)
            {
                var value = ModelState[modelStateKey];
                foreach (var error in value.Errors)
                {
                    Console.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                }
            }

            var users = userService.GetAllUsers();
            ViewBag.UserId = new SelectList(users, "Id", "Id", bankAccount.UserId);

            return View(bankAccount);
        }


        // ✅ Update Bank Account (GET)
        [HttpGet]
        public IActionResult UpdateBankAccount(long accountId)
        {

            ViewData["Title"] = "Add New Bank Account";
            var users = userService.GetAllUsers();
            if (users == null || !users.Any())
            {
                throw new Exception("No users found. Please ensure the database contains user records.");
            }

            IEnumerable<Users> users1 = userService.GetAllUsers();

            ViewBag.UserId = new SelectList(users1, "Id", "Id");
            var bankAccount = bankAccountService.GetBankAccountById(accountId);
            if (bankAccount == null)
            {
                return NotFound("Bank account not found");
            }
            return View(bankAccount);
        }


        // ✅ Update Bank Account (POST)
        [HttpPost]
        public IActionResult UpdateBankAccount(BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bankAccountService.UpdateBankAccount(bankAccount);
                    return RedirectToAction("ViewBankAccounts");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while updating the bank account. Please try again.");
                }
            }
            var users = userService.GetAllUsers();
            ViewBag.UserId = new SelectList(users, "Id", "Id", bankAccount.UserId);
            return View(bankAccount);
        }

        // ✅ Delete Bank Account (GET) — Confirmation Page
        [HttpGet]
        public IActionResult DeleteBankAccount(long accountId)
        {
            var bankAccount = bankAccountService.GetBankAccountById(accountId);
            if (bankAccount == null)
            {
                return NotFound("Bank account not found");
            }
            return View(bankAccount);
        }

        // ✅ Delete Bank Account (POST)
        [HttpPost, ActionName("DeleteBankAccount")]
        public IActionResult DeleteBankAccountConfirmed(long accountId)
        {
            try
            {
                bankAccountService.DeleteBankAccount(accountId);
                return RedirectToAction("ViewBankAccounts");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while deleting the bank account. Please try again.");
            }
            return RedirectToAction("ViewBankAccounts");
        }
    }
}
