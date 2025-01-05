using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MINIBANK.Models;
using MINIBANK.Services.Interface;

namespace MINIBANK.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;

        // Constructor with dependency injection
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        // Index page
        public IActionResult Index()
        {
            return View();
        }

        // Add User - GET----------------------------------------------------------
        [Authorize(Roles = "User")]
        public IActionResult AddUser()
        {
            ViewData["Title"] = "Add New User";
            Debug.WriteLine(ViewData["Title"]); // This will print the value to the debug console
            return View();
        }

        // Add User - POST
        [HttpPost]
        public IActionResult AddUser(Users user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    userService.AddUserService(user);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while adding the user. Please try again.");
                }
            }
            return View(user);
        }
        // GET: UpdateUser
        [HttpGet]
        public IActionResult UpdateUser(int id)
        {
            var user = userService.GetUserByIdService(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            return View(user);  // Make sure the View receives the correct model (Users)
        }

        // POST: UpdateUser
        [HttpPost]
        public IActionResult UpdateUser(Users user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    userService.UpdateUserService(user); // Update the user
                    return RedirectToAction("Index"); // Redirect to user list after update
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while updating the user. Please try again.");
                }
            }
            return View(user); // Return to the same view with user data if model is invalid
        }




        // Delete User-------------------------------------------------------
        // GET: DeleteUser (Shows confirmation)
        public IActionResult DeleteUser(int id)
        {
            var user = userService.GetUserByIdService(id);
            if (user == null)
            {
                return RedirectToAction("Index"); // If user not found, redirect to index
            }

            return View(user); // Show confirmation page with user details
        }

        // POST: DeleteUser (Confirms deletion)
        [HttpPost]
        [ActionName("DeleteUser")] // Ensures this method handles the POST request for deletion
        public IActionResult ConfirmDelete(int id)
        {
            userService.DeleteUserService(id); // Delete the user
            return RedirectToAction("Index"); // Redirect to the user list after deletion
        }
        // Get All Users-------------------------------------------------------
        public IActionResult GetAllUsers()
        {
            var users = userService.GetAllUsersService();
            if (users == null || !users.Any())
            {
                ViewBag.Message = "No users found.";
            }
            return View(users);
        }
        // Get User by ID-------------------------------------------------------
        //public IActionResult GetUser(int id)
        //{
        //    var user = userService.GetUserByIdService(id);
        //    if (user == null)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    return View(user);
        //}
    }
}
