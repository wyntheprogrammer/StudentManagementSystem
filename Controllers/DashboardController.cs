using Microsoft.AspNetCore.Mvc;

namespace StudentManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}