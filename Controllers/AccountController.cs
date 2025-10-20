using Microsoft.AspNetCore.Mvc;

namespace StudentManagementSystem.Controllers
{
    public class AccountController : Controller
    {

        private readonly ILogger<AccountController> _logger;
        private readonly AppDbContext _context;

        public AccountController(ILogger<AccountController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email  == email && u.Password == password);

            if (user != null) 
            {
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserRole", user.Role);

                TempData["SuccessMessage"] = "Welcome Back " + user.Name + "!";
                return RedirectToAction("Index", "Dashboard");
            }

            TempData["ErrorMessage"] = "Invalid email or password.";
            return RedirectToAction("Login", "Account");

        }


        public IActionResult Logout(){
            HttpContext.Session.Clear();

            TempData["SuccessMessage"] = "You have been logged out.";

            return RedirectToAction("Login", "Account");
        }
    }
}
