using Microsoft.AspNetCore.Mvc;

namespace StudentManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // if (email == "admin@gmail.com" && password == "123")
            // {
            //     return RedirectToAction("Index", "Dashboard");
            // }

            // TempData["ErrorMessage"] = "Invalid. Please check the form for errors.";
            // return View();

            return RedirectToAction("Index", "Dashboard");

        }
    }
}
