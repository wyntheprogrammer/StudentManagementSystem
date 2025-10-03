using Microsoft.AspNetCore.Mvc;

namespace StudentManagementSystem.Controllers
{
    public class EnrollmentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}