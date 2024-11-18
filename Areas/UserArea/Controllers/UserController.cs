using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaWeb.Data;
using SocialMediaWeb.Models;
using System.Net.WebSockets;

namespace SocialMediaWeb.Areas.UserArea.Controllers
{
    [Area("UserArea")]
    public class UserController : Controller
    {
        private readonly DBContext _dbContext;
        private readonly ILogger<UserController> _logger;

        public UserController(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser(User model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Users.Add(model); 
                    _dbContext.SaveChanges();    
                    return RedirectToAction("Login", "User", new { area = "UserArea"}); 
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while registering. Please try again.");
                }
            }
            return View(model);
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string usernameOrEmail, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(u =>
                (u.Username == usernameOrEmail || u.Email == usernameOrEmail) && u.PasswordHash == password);

            if (user != null)
            {
                TempData["UserId"] = user.Id;
                return RedirectToAction("Profile");
            }

            return View();
        }

        public IActionResult Logout()
        {
            TempData.Clear();
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Profile()
        {
            if (TempData["UserId"] is int userId)
            {
                var user = await _dbContext.Users
                    .Include(u => u.Posts)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user != null)
                {
                    return View(user);
                }
            }
            return RedirectToAction("Login");
        }
    }
}
